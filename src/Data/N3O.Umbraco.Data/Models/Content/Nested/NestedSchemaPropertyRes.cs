using N3O.Umbraco.Data.Lookups;

namespace N3O.Umbraco.Data.Models;

public class NestedSchemaPropertyRes {
    public string Alias { get; set; }
    public PropertyType Type { get; set; }
    public ContentPropertyConfigurationRes Configuration { get; set; }
}