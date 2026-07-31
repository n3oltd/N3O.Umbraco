namespace N3O.Umbraco.Scheduler;

public static class SchedulerConstants {
    public static class Parameters {
        public const string Attempt = "n3o_attempt";
        public const string Culture = "culture";
        public const string Queue = "n3o_queue";
        public const string Signature = "n3o_signature";
    }
    
    public static class Queues {
        public const string Default = "default";
        public const string LongJobs = "long_jobs";
    }
    
    public static class Workers {
        public const string DefaultWorker = "DefaultWorker";
        public const string LongJobsWorker = "LongJobsWorker";
    }
}