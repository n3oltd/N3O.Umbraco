using Hangfire;
using Hangfire.Dashboard;
using Hangfire.SqlServer;
using Humanizer.Bytes;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Mediator;
using N3O.Umbraco.Scheduler.Attributes;
using N3O.Umbraco.Scheduler.Commands;
using N3O.Umbraco.Scheduler.Filters;
using N3O.Umbraco.Scheduler.Models;
using N3O.Umbraco.Utilities;
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
    private static readonly string HangfireDashboard = nameof(HangfireDashboard);

    public void Compose(IUmbracoBuilder builder) {
        builder.Services.AddSingleton<IJobUrlProvider, JobUrlProvider>();
        builder.Services.AddTransient<IBackgroundJob, BackgroundJob>();
        
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
                opt.WorkerCount = 1;
            });

            builder.Services.AddHangfireServer(opt => {
                opt.ServerName = SchedulerConstants.Workers.LongJobsWorker;
                opt.Queues = [SchedulerConstants.Queues.LongJobs];
                opt.WorkerCount = 1;
            });

            AddAuthorizedUmbracoDashboard(builder);

            // https://discuss.hangfire.io/t/jobstorage-current-property-value-has-not-been-initialized/884
            JobStorage.Current = new SqlServerStorage(connectionString);

            builder.Components().Append<CleanupStaleRecurringJobsComponent>();
            builder.Components().Append<RegisterRecurringJobsComponent>();
        }
    }

    private void AddAuthorizedUmbracoDashboard(IUmbracoBuilder builder) {
        // In v17 the backoffice uses JWT bearer tokens for API calls, so RequireAuthorization
        // with BackOfficeAuthenticationType issues a Bearer challenge that blocks iframe navigation.
        // Instead, gate access inside the IDashboardAuthorizationFilter using the HttpContext that
        // Umbraco's auth middleware (running earlier in UmbracoPipelineFilter) has already populated
        // from the __Host-umbAccessToken cookie. Only authenticated backoffice users pass.
        builder.Services.Configure<UmbracoPipelineOptions>(opt => {
            var filter = new UmbracoPipelineFilter(HangfireDashboard);
            filter.Endpoints = app => app.UseEndpoints(endpoints => {
                // AllowAnonymous bypasses Umbraco v17's fallback auth policy for
                // /umbraco/backoffice/* (which issues a Bearer challenge blocking iframe
                // navigation). UmbracoAuthorizationFilter handles auth instead.
                endpoints.MapHangfireDashboard("/umbraco/backoffice/hangfire",
                                               new DashboardOptions {
                                                   AppPath = null,
                                                   Authorization = new[] { new UmbracoAuthorizationFilter() }
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
    
    // https://github.com/nul800sebastiaan/Cultiv.Hangfire/issues/5
    // In v17 the UMB_UCONTEXT cookie uses SameSite=Strict which Chrome does not send for
    // iframe document navigations, causing BackOfficeAuthenticationType to challenge with
    // 401. AllowAnonymous on the endpoint (above) bypasses that middleware challenge; this
    // filter then checks __Host-umbAccessToken, which has no SameSite=Strict restriction
    // and is always sent for same-origin iframe requests. Its __Host- prefix guarantees
    // it can only be set by this HTTPS server, so its presence is a reliable auth indicator.
    public class UmbracoAuthorizationFilter : IDashboardAuthorizationFilter {
        public bool Authorize(DashboardContext context) {
            return context is AspNetCoreDashboardContext aspCtx &&
                   aspCtx.HttpContext.Request.Cookies.ContainsKey("__Host-umbAccessToken");
        }
    }
}
