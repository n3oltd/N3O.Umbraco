using N3O.Umbraco.Lookups;

namespace N3O.Umbraco.Data.Lookups;

public class ContentMetadataRes : NamedLookupRes {
    public bool AutoSelected { get; set; }
    public int DisplayOrder { get; set; }
}
