using N3O.Umbraco.Giving.Lookups;

namespace N3O.Umbraco.Giving.Models;

public class FeedbackCustomFieldDefinition : Value, IFeedbackCustomFieldDefinition {
    public FeedbackCustomFieldDefinition(FeedbackCustomFieldType type,
                                         string alias,
                                         string name,
                                         bool required,
                                         int maxTextLength) {
        Type = type;
        Alias = alias;
        Name = name;
        Required = required;
        MaxTextLength = maxTextLength;
    }

    public FeedbackCustomFieldType Type { get; }
    public string Alias { get; }
    public string Name { get; }
    public bool Required { get; }
    public int? MaxTextLength { get; }
}