using Hangfire;
using Hangfire.Storage;
using Microsoft.Extensions.Logging;
using System;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.Services;

namespace N3O.Umbraco.Scheduler;

public class CleanupStaleRecurringJobsComponent : IComponent {
    private readonly IRuntimeState _runtimeState;
    private readonly ILogger<CleanupStaleRecurringJobsComponent> _logger;

    public CleanupStaleRecurringJobsComponent(IRuntimeState runtimeState,
                                              ILogger<CleanupStaleRecurringJobsComponent> logger) {
        _runtimeState = runtimeState;
        _logger = logger;
    }

    public void Initialize() {
        if (_runtimeState.Level != RuntimeLevel.Run) {
            return;
        }

        try {
            using var connection = JobStorage.Current.GetConnection();
            var jobs = connection.GetRecurringJobs();
            var removed = 0;

            foreach (var job in jobs) {
                if (!IsStale(job, out var reason)) {
                    continue;
                }

                _logger.LogWarning("Removing stale recurring job '{JobId}': {Reason}", job.Id, reason);

                RecurringJob.RemoveIfExists(job.Id);
                removed++;
            }

            if (removed > 0) {
                _logger.LogInformation("Removed {Count} stale recurring job(s) on startup", removed);
            }
        } catch (Exception ex) {
            _logger.LogError(ex, "Failed to clean up stale recurring jobs");
        }
    }

    public void Terminate() { }

    private static bool IsStale(RecurringJobDto job, out string reason) {
        reason = null;

        if (job.LoadException != null || job.Job == null) {
            reason = $"Hangfire could not load the job ({job.LoadException?.Message ?? "Job is null"})";
            return true;
        }

        if (job.Job.Method?.DeclaringType != typeof(JobTrigger) ||
            job.Job.Method.Name != nameof(JobTrigger.TriggerAsync) ||
            job.Job.Args == null ||
            job.Job.Args.Count < 2) {
            return false;
        }

        var triggerKey = job.Job.Args[1] as string;

        if (string.IsNullOrEmpty(triggerKey)) {
            return false;
        }

        try {
            TriggerKey.Parse(triggerKey);
            return false;
        } catch (InvalidOperationException ex) {
            reason = ex.Message;
            return true;
        } catch {
            return false;
        }
    }
}
