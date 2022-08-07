using N3O.Umbraco.Lookups;
using N3O.Umbraco.Utilities;

namespace N3O.Umbraco.Data.Lookups;

public class WorkbookFormat : NamedLookup {
    public WorkbookFormat(string id, string name, string contentType, bool supportsPasswordProtection)
        : base(id, name) {
        ContentType = contentType;
        SupportsPasswordProtection = supportsPasswordProtection;
    }

    public string ContentType { get; }
    public bool SupportsPasswordProtection { get; }

    public string AppendFileExtension(string name) {
        var filename = $"{name}.{FileUtility.GetFileExtension(ContentType)}";

        return filename;
    }
}

public class WorkbookFormats : StaticLookupsCollection<WorkbookFormat> {
    public static readonly WorkbookFormat Csv = new("csv", "CSV", DataConstants.ContentTypes.Csv, false);
    public static readonly WorkbookFormat Excel = new("excel", "Excel", DataConstants.ContentTypes.Excel, true);
}
