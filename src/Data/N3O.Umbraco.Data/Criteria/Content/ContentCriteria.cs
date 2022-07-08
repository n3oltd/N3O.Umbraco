using N3O.Umbraco.Attributes;

namespace N3O.Umbraco.Data.Criteria; 

public class ContentCriteria {
    [Name("Content Type Alias")]
    public string ContentTypeAlias { get; set; }
}