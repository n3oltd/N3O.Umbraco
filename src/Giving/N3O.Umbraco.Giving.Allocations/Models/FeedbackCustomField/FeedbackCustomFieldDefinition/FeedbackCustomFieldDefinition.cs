using N3O.Umbraco.Extensions;
using N3O.Umbraco.Giving.Allocations.Lookups;

namespace N3O.Umbraco.Giving.Allocations.Models;

public class FeedbackCustomFieldDefinition : Value, IFeedbackCustomFieldDefinition {
    public FeedbackCustomFieldDefinition(FeedbackCustomFieldType type,
                                         string alias,
                                         string name,
                                         bool required,
                                         int textMaxLength) {
        Type = type;
        Alias = alias;
        Name = name;
        Required = required;
        TextMaxLength = textMaxLength;
    }

    public FeedbackCustomFieldDefinition(IFeedbackCustomFieldDefinition customFieldDefinition)
        : this(customFieldDefinition.Type,
               customFieldDefinition.Alias,
               customFieldDefinition.Name,
               customFieldDefinition.Required,
               customFieldDefinition.TextMaxLength.GetValueOrThrow()) { }

    public FeedbackCustomFieldType Type { get; }
    public string Alias { get; }
    public string Name { get; }
    public bool Required { get; }
    public int? TextMaxLength { get; }
}