using Hangfire;
using Hangfire.SqlServer;
using Humanizer.Bytes;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Mediator;
using N3O.Umbraco.Scheduler.Attributes;
using N3O.Umbraco.Scheduler.Commands;
using N3O.Umbraco.Scheduler.Filters;
using N3O.Umbraco.Scheduler.Models;
using N3O.Umbraco.Utilities;
using OpenIddict.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.DependencyInjection;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Web.Common.ApplicationBuilder;
using UmbracoConstants = Umbraco.Cms.Core.Constants;

namespace N3O.Umbraco.Scheduler;

public class SchedulerComposer : IComposer {
    public void Compose(IUmbracoBuilder builder) {
        builder.Services.AddSingleton<IJobUrlProvider, JobUrlProvider>();
        builder.Services.AddTransient<IBackgroundJob, BackgroundJob>();

        var settings = builder.Config.GetSection(SchedulerSettings.SectionName).Get<SchedulerSettings>() ??
                       new SchedulerSettings();
        builder.Services.AddSingleton(settings);

        var connectionString = builder.Config.GetConnectionString(UmbracoConstants.System.UmbracoConnectionName);

        if (connectionString.HasValue()) {
            var sqlStorageOptions = new SqlServerStorageOptions();
            sqlStorageOptions.CommandBatchMaxTimeout = TimeSpan.FromMinutes(5);
            sqlStorageOptions.SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5);
            sqlStorageOptions.QueuePollInterval = TimeSpan.Zero;
            sqlStorageOptions.UseRecommendedIsolationLevel = true;
            sqlStorageOptions.DisableGlobalLocks = true;

            builder.Services.AddHangfire(opt => {
                opt.SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                   .UseSimpleAssemblyNameTypeSerializer()
                   .UseRecommendedSerializerSettings()
                   .UseSqlServerStorage(connectionString, sqlStorageOptions)
                   .WithJobExpirationTimeout(TimeSpan.FromDays(3))
                   .UseFilter(new JobLoggerFilter())
                   .UseMaxArgumentSizeToRender((int) ByteSize.FromKilobytes(256).Bytes)
                   .UseMaxLinesInExceptionDetails(200);
            });
            
            builder.Services.AddHangfireServer(opt => {
                opt.ServerName = SchedulerConstants.Workers.DefaultWorker;
                opt.Queues = [SchedulerConstants.Queues.Default];
                opt.WorkerCount = settings.DefaultWorkerCount;
            });

            builder.Services.AddHangfireServer(opt => {
                opt.ServerName = SchedulerConstants.Workers.LongJobsWorker;
                opt.Queues = [SchedulerConstants.Queues.LongJobs];
                opt.WorkerCount = settings.LongJobsWorkerCount;
            });

            AddHangfireDashboardAuthentication(builder);
            AddAuthorizedUmbracoDashboard(builder);

            // https://discuss.hangfire.io/t/jobstorage-current-property-value-has-not-been-initialized/884
            JobStorage.Current = new SqlServerStorage(connectionString);

            builder.Components().Append<CleanupStaleRecurringJobsComponent>();
            builder.Components().Append<RegisterRecurringJobsComponent>();
            builder.Components().Append<ValidateRunsWhereQueuedComponent>();
        }
    }

    private void AddHangfireDashboardAuthentication(IUmbracoBuilder builder) {
        builder.Services
               .AddAuthentication()
               .AddCookie(SchedulerConstants.Dashboard.CookieScheme, opt => {
                   opt.Cookie.Name = SchedulerConstants.Dashboard.CookieName;
                   opt.Cookie.Path = "/";
                   opt.Cookie.HttpOnly = true;
                   opt.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                   opt.Cookie.SameSite = SameSiteMode.Strict;
                   opt.SlidingExpiration = true;
               });

        builder.Services.AddSingleton<HangfireDashboardCookieIssuer>();

        builder.Services.Configure<OpenIddictServerOptions>(opt => {
            opt.Handlers.Add(OpenIddictServerHandlerDescriptor
                             .CreateBuilder<OpenIddictServerEvents.GenerateTokenContext>()
                             .UseSingletonHandler<HangfireDashboardCookieIssuer>()
                             .Build());

            opt.Handlers.Add(OpenIddictServerHandlerDescriptor
                             .CreateBuilder<OpenIddictServerEvents.ApplyRevocationResponseContext>()
                             .UseSingletonHandler<HangfireDashboardCookieIssuer>()
                             .Build());
        });
    }

    private void AddAuthorizedUmbracoDashboard(IUmbracoBuilder builder) {
        builder.Services.Configure<UmbracoPipelineOptions>(opt => {
            var filter = new UmbracoPipelineFilter(SchedulerConstants.Dashboard.Name);
            filter.Endpoints = app => app.UseEndpoints(endpoints => {
                endpoints.MapHangfireDashboard("/umbraco/backoffice/hangfire",
                                               new DashboardOptions {
                                                   AppPath = null,
                                                   AsyncAuthorization = new[] { new HangfireDashboardAuthorizationFilter() }
                                               })
                         .AllowAnonymous();
            })
            .UseHangfireDashboard();

            opt.AddFilter(filter);
        });
    }

    public class RegisterRecurringJobsComponent : IAsyncComponent {
        private readonly IRuntimeState _runtimeState;
        private readonly Lazy<IMediator> _mediator;
        private readonly Lazy<IUmbracoContextFactory> _umbracoContextFactory;

        public RegisterRecurringJobsComponent(IRuntimeState runtimeState, Lazy<IMediator> mediator, Lazy<IUmbracoContextFactory> umbracoContextFactory) {
            _runtimeState = runtimeState;
            _mediator = mediator;
            _umbracoContextFactory = umbracoContextFactory;
        }
    
        public Task InitializeAsync(bool isRestarting, CancellationToken cancellationToken) {
            if (_runtimeState.Level == RuntimeLevel.Run) {
                var recurringJobTypes = OurAssemblies.GetTypes(t => t.IsConcreteClass() &&
                                                                    t.HasAttribute<RecurringJobAttribute>())
                                                     .ToList();

                if (recurringJobTypes.Any()) {
                    using (_umbracoContextFactory.Value.EnsureUmbracoContext()) {
                        var jobReqs = new List<QueueRecurringJobReq>();

                        foreach (var jobType in recurringJobTypes) {
                            if (!jobType.ImplementsGenericInterface(typeof(IRequestHandler<,,>))) {
                                throw new
                                    Exception("Recurring job attribute can only be applied to classes that inherit IRequestHandler<,,>");
                            }

                            var attribute = jobType.GetCustomAttribute<RecurringJobAttribute>();
                            var requestType = jobType.GetParameterTypesForGenericInterface(typeof(IRequestHandler<,,>))
                                                     .First();

                            var triggerKey = TriggerKey.Generate(requestType, typeof(None));

                            var jobReq = new QueueRecurringJobReq();

                            jobReq.CronExpression = attribute.CronExpression;
                            jobReq.JobName = attribute.JobName;
                            jobReq.TriggerKey = triggerKey;

                            jobReqs.Add(jobReq);
                        }

                        var req = new RegisterRecurringJobsReq();
                        req.Jobs = jobReqs;

                        _mediator.Value
                                 .SendAsync<RegisterRecurringJobsCommand, RegisterRecurringJobsReq>(req)
                                 .GetAwaiter()
                                 .GetResult();
                    }
                }
            }

            return Task.CompletedTask;
        }

        public Task TerminateAsync(bool isRestarting, CancellationToken cancellationToken) => Task.CompletedTask;
    }

    public class ValidateRunsWhereQueuedComponent : IAsyncComponent {
        private readonly IRuntimeState _runtimeState;

        public ValidateRunsWhereQueuedComponent(IRuntimeState runtimeState) {
            _runtimeState = runtimeState;
        }

        public Task InitializeAsync(bool isRestarting, CancellationToken cancellationToken) {
            if (_runtimeState.Level == RuntimeLevel.Run) {
                var runsWhereQueuedTypes = OurAssemblies.GetTypes(t => t.IsConcreteClass() &&
                                                                       t.HasAttribute<RunsWhereQueuedAttribute>());

                foreach (var commandType in runsWhereQueuedTypes) {
                    if (!commandType.InheritsGenericClass(typeof(Request<,>))) {
                        throw new Exception("Runs where queued attribute can only be applied to classes that " +
                                            $"inherit Request<,> but was applied to {commandType.Name}");
                    }
                }
            }

            return Task.CompletedTask;
        }

        public Task TerminateAsync(bool isRestarting, CancellationToken cancellationToken) => Task.CompletedTask;
    }
}
