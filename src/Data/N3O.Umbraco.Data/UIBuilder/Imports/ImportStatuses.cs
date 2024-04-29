namespace N3O.Umbraco.Data.UIBuilder;

public static class ImportStatuses {
    public static readonly string Abandoned = "Abandoned";
    public static readonly string Error = "Error";
    public static readonly string Queued = "Queued";
    public static readonly string Saved = "Saved";
    public static readonly string SavedAndPublished = "Saved and Published";

    public static readonly string[] All = {
        Abandoned, Error, Queued, Saved, SavedAndPublished
    };
}
