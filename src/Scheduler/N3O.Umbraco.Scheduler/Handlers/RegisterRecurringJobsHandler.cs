using Hangfire;
using Hangfire.Storage;
using N3O.Umbraco;
using N3O.Umbraco.Json;
using N3O.Umbraco.Mediator;
using N3O.Umbraco.Scheduler;
using N3O.Umbraco.Scheduler.Commands;
using N3O.Umbraco.Scheduler.Models;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Karakoram.Scheduler.Domain.Handlers {
    public class RegisterRecurringJobsHandler :
        IRequestHandler<RegisterRecurringJobsCommand, RegisterRecurringJobsReq, None> {
        private readonly JobStorage _jobStorage;
        private readonly IJsonProvider _jsonProvider;

        public RegisterRecurringJobsHandler(JobStorage jobStorage, IJsonProvider jsonProvider) {
            _jobStorage = jobStorage;
            _jsonProvider = jsonProvider;
        }

        public Task<None> Handle(RegisterRecurringJobsCommand req, CancellationToken cancellationToken) {
            var existingJobs = _jobStorage.GetConnection().GetRecurringJobs().ToList();
        
            foreach (var hangfireJob in existingJobs) {
                RecurringJob.RemoveIfExists(hangfireJob.Id);
            }

            var modelJson = _jsonProvider.SerializeObject(None.Empty);

            foreach (var job in req.Model.Jobs) {
                Expression<Func<JobTrigger, Task>> triggerActionAsync = jobTrigger =>
                    jobTrigger.TriggerAsync(job.JobName,
                                            job.TriggerKey,
                                            modelJson,
                                            null);

                RecurringJob.AddOrUpdate(triggerActionAsync, job.CronExpression, TimeZoneInfo.Utc);
            }

            return Task.FromResult(None.Empty);
        }
    }
}
