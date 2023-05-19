using N3O.Umbraco.Giving.Lookups;

namespace N3O.Umbraco.Giving.Models;

public class FeedbackCustomFieldDefinitionRes : IFeedbackCustomFieldDefinition {
    public FeedbackCustomFieldType Type { get; set; }
    public string Alias { get; set; }
    public string Name { get; set; }
    public bool Required { get; set; }
    public int? TextMaxLength { get; set; }
}