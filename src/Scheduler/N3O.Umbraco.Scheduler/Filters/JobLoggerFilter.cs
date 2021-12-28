using Hangfire.Client;
using Hangfire.Common;
using Hangfire.Server;
using Microsoft.Extensions.Logging;

namespace N3O.Umbraco.Scheduler.Filters {
    public class JobLoggerFilter : JobFilterAttribute, IClientFilter, IServerFilter {
        private ILogger _logger;

        public void OnCreating(CreatingContext filterContext) {
            _logger ??= GetLogger();

            _logger.Log(LogLevel.Information,
                        "Job is being created for {JobType} with arguments {JobArguments}",
                        filterContext.Job.Type.Name,
                        filterContext.Job.Args);
        }

        public void OnCreated(CreatedContext filterContext) {
            _logger ??= GetLogger();

            _logger.Log(LogLevel.Information,"Job {JobId} has been created", filterContext.BackgroundJob.Id);
        }

        public void OnPerforming(PerformingContext filterContext) {
            _logger ??= GetLogger();

            _logger.Log(LogLevel.Information,"Job {JobId} is running", filterContext.BackgroundJob.Id);
        }

        public void OnPerformed(PerformedContext filterContext) {
            _logger ??= GetLogger();

            _logger.Log(LogLevel.Information,"Job {JobId} has completed", filterContext.BackgroundJob.Id);

            if (filterContext.Exception != null) {
                _logger.Log(LogLevel.Error,
                            filterContext.Exception,
                            "Job {JobId} failed due to an exception",
                            filterContext.BackgroundJob.Id);
            }

            _logger = null;
        }

        private ILogger GetLogger() {
            var factory = LoggerFactory.Create(b => b.AddConsole());
            var logger = factory.CreateLogger<JobLoggerFilter>();

            return logger;
        }
    }
}