using Hangfire.Client;
using Hangfire.Common;
using Hangfire.Server;
using Serilog;
using System;

namespace Karakoram.Scheduler.Domain.Attributes {
    public class JobLoggerAttribute : JobFilterAttribute, IClientFilter, IServerFilter {
        private ILogger _logger;

        public void OnCreating(CreatingContext filterContext) {
            if (_logger == null) {
                _logger = GetLogger();
            }

            _logger.Information("Job is being created for {JobType} with arguments {JobArguments}",
                                filterContext.Job.Type.Name,
                                filterContext.Job.Args);
        }

        public void OnCreated(CreatedContext filterContext) {
            if (_logger == null) {
                _logger = GetLogger();
            }

            _logger.Information("Job {JobId} has been created", filterContext.BackgroundJob.Id);
        }

        public void OnPerforming(PerformingContext filterContext) {
            if (_logger == null) {
                _logger = GetLogger();
            }

            _logger.Information("Job {JobId} is running", filterContext.BackgroundJob.Id);
        }

        public void OnPerformed(PerformedContext filterContext) {
            if (_logger == null) {
                _logger = GetLogger();
            }

            _logger.Information("Job {JobId} has completed", filterContext.BackgroundJob.Id);

            if (filterContext.Exception != null) {
                _logger.Error(filterContext.Exception,
                              "Job {JobId} failed due to an exception",
                              filterContext.BackgroundJob.Id);
            }

            _logger = null;
        }

        private ILogger GetLogger() {
            return Log.ForContext(GetType())
                      .ForContext("HangfireRequestId", Guid.NewGuid());
        }
    }
}
