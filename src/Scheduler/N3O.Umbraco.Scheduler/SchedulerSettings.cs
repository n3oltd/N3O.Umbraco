namespace N3O.Umbraco.Scheduler;

public class SchedulerSettings {
    public const string SectionName = "N3O:Scheduler";

    public int DefaultWorkerCount { get; set; } = 1;
    public int LongJobsWorkerCount { get; set; } = 1;
    public int JobTimeoutMinutes { get; set; } = 30;
}
