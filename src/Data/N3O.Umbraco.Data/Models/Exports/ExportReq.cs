using N3O.Umbraco.Attributes;
using N3O.Umbraco.Data.Lookups;
using System.Collections.Generic;

namespace N3O.Umbraco.Data.Models;

public class ExportReq {
    [Name("Format")]
    public WorkbookFormat Format { get; set; }

    [Name("Include Unpublished")]
    public bool? IncludeUnpublished { get; set; }

    [Name("Metadata")]
    public IEnumerable<ContentMetadata> Metadata { get; set; }
    
    [Name("Properties")]
    public IEnumerable<string> Properties { get; set; }
}
