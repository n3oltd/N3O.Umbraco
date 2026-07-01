namespace N3O.Umbraco.Scheduler;

public static class SchedulerConstants {
    public static class Dashboard {
        public const string CookieName = "__Host-N3O.Hangfire";
        public const string CookieScheme = "N3O.Hangfire.CookieScheme";
        public const string IdentityAuthenticationType = "HangfireAllowed";
        public const string Name = "HangfireDashboard";
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