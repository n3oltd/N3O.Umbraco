using N3O.Umbraco.Attributes;

namespace N3O.Umbraco.Data.Criteria;

public class ContentTypeCriteria {
    [Name("Alias")]
    public string Alias { get; set; }
}
