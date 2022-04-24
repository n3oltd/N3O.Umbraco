namespace N3O.Umbraco.Data.Konstrukt {
    public static class ImportStatuses {
        public static readonly string Abandoned = "Abandoned";
        public static readonly string Error = "Error";
        public static readonly string Imported = "Imported";
        public static readonly string ImportedWithWarning = "Imported with Warning";
        public static readonly string Queued = "Queued";

        public static readonly string[] All = {
            Abandoned, Error, Imported, ImportedWithWarning, Queued
        };
    }
}