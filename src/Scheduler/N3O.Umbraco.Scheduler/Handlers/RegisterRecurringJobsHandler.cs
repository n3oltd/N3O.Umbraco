using Hangfire;
using Hangfire.Storage;
using N3O.Umbraco.Json;
using N3O.Umbraco.Mediator;
using N3O.Umbraco.Scheduler.Commands;
using N3O.Umbraco.Scheduler.Models;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Scheduler.Handlers;

public class RegisterRecurringJobsHandler :
    IRequestHandler<RegisterRecurringJobsCommand, RegisterRecurringJobsReq, None> {
    private readonly IJsonProvider _jsonProvider;

    public RegisterRecurringJobsHandler(IJsonProvider jsonProvider) {
        _jsonProvider = jsonProvider;
    }

    public Task<None> Handle(RegisterRecurringJobsCommand req, CancellationToken cancellationToken) {
        var existingJobs = JobStorage.Current.GetConnection().GetRecurringJobs().ToList();
    
        foreach (var hangfireJob in existingJobs) {
            RecurringJob.RemoveIfExists(hangfireJob.Id);
        }

        var modelJson = _jsonProvider.SerializeObject(None.Empty);

        foreach (var job in req.Model.Jobs) {
            var options = new RecurringJobOptions();
            options.MisfireHandling = MisfireHandlingMode.Relaxed;
            options.TimeZone = TimeZoneInfo.Utc;
            
            RecurringJob.AddOrUpdate<JobTrigger>(job.GetJobId(),
                                                 j => j.TriggerAsync(job.JobName, job.TriggerKey, modelJson, null),
                                                 job.CronExpression,
                                                 options);
        }

        return Task.FromResult(None.Empty);
    }
}
