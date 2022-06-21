using N3O.Umbraco.Attributes;

namespace N3O.Umbraco.Scheduler.Models;

public class QueueRecurringJobReq {
    [Name("Cron Expression")]
    public string CronExpression { get; set; }

    [Name("Job Name")]
    public string JobName { get; set; }

    [Name("Trigger Key")]
    public string TriggerKey { get; set; }
}
