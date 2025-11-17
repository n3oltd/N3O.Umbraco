namespace N3O.Umbraco.Scheduler;

public static class SchedulerConstants {
    public static class Parameters {
        public const string Culture = "culture";
    }
    
    public static class Queues {
        public const string Default = "default";
        public const string Exports = "exports";
    }
    
    public static class Workers {
        public const string DefaultWorker = "DefaultWorker";
        public const string ExportsWorker = "ExportsWorker";
    }
}