namespace N3O.Umbraco.Scheduler;

public static class SchedulerConstants {
    public static class Config {
        public const string DefaultWorkerCount = "N3O:Scheduler:DefaultWorkerCount";
        public const string LongJobsWorkerCount = "N3O:Scheduler:LongJobsWorkerCount";
        public const string JobTimeoutMinutes = "N3O:Scheduler:JobTimeoutMinutes";
    }

    public static class Defaults {
        public const int WorkerCount = 1;
        public const int JobTimeoutMinutes = 30;
    }

    public static class Parameters {
        public const string Culture = "culture";
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