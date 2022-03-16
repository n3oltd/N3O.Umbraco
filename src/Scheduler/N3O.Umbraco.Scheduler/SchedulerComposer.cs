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
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.DependencyInjection;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Web.BackOffice.Authorization;
using Umbraco.Cms.Web.Common.ApplicationBuilder;
using UmbracoConstants = Umbraco.Cms.Core.Constants;

namespace N3O.Umbraco.Scheduler {
    public class SchedulerComposer : IComposer {
        private const string HangfireDashboard = nameof(HangfireDashboard);
    
        public void Compose(IUmbracoBuilder builder) {
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
                       .UseFilter(new JobLoggerFilter())
                       .UseMaxArgumentSizeToRender((int) ByteSize.FromKilobytes(256).Bytes)
                       .UseMaxLinesInExceptionDetails(200);
                });

                builder.Services.AddHangfireServer();

                AddAuthorizedUmbracoDashboard(builder);

                // https://discuss.hangfire.io/t/jobstorage-current-property-value-has-not-been-initialized/884
                JobStorage.Current = new SqlServerStorage(connectionString);

                builder.Components().Append<RegisterRecurringJobsComponent>();
            }
        }

        private void AddAuthorizedUmbracoDashboard(IUmbracoBuilder builder) {
            builder.Services.AddAuthorization(opt => {
                opt.AddPolicy(HangfireDashboard, policy => {
                    policy.AuthenticationSchemes.Add(UmbracoConstants.Security.BackOfficeAuthenticationType);
                    policy.Requirements.Add(new SectionRequirement(UmbracoConstants.Applications.Settings));
                });
            });

            builder.Services.Configure<UmbracoPipelineOptions>(opt => {
                var filter = new UmbracoPipelineFilter(HangfireDashboard);
                filter.Endpoints = app => app.UseEndpoints(endpoints => {
                    endpoints.MapHangfireDashboard("/umbraco/backoffice/hangfire",
                                                   new DashboardOptions {
                                                       AppPath = null,
                                                       Authorization = new[] { new UmbracoAuthorizationFilter() }
                                                   })
                             .RequireAuthorization(HangfireDashboard);
                })
                .UseHangfireDashboard();
            
                opt.AddFilter(filter);
            });
        }

        public class RegisterRecurringJobsComponent : IComponent {
            private readonly IRuntimeState _runtimeState;
            private readonly Lazy<IMediator> _mediator;

            public RegisterRecurringJobsComponent(IRuntimeState runtimeState, Lazy<IMediator> mediator) {
                _runtimeState = runtimeState;
                _mediator = mediator;
            }
        
            public void Initialize() {
                if (_runtimeState.Level == RuntimeLevel.Run) {
                    var recurringJobTypes = OurAssemblies.GetTypes(t => t.IsConcreteClass() &&
                                                                        t.HasAttribute<RecurringJobAttribute>())
                                                         .ToList();

                    if (recurringJobTypes.Any()) {
                        var jobReqs = new List<QueueRecurringJobReq>();

                        foreach (var jobType in recurringJobTypes) {
                            if (!jobType.ImplementsGenericInterface(typeof(IRequestHandler<,,>))) {
                                throw new Exception("Recurring job attribute can only be applied to classes that inherit IRequestHandler<,,>");
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
            
            public void Terminate() { }
        }
        
        // https://github.com/nul800sebastiaan/Cultiv.Hangfire/issues/5
        public class UmbracoAuthorizationFilter : IDashboardAuthorizationFilter {
            public bool Authorize(DashboardContext context) => true;
        }
    }
}
