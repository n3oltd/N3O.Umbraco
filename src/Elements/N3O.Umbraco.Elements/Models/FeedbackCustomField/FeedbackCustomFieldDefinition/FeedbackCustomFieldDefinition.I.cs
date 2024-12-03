using N3O.Umbraco.Elements.Lookups;

namespace N3O.Umbraco.Elements.Models;

public interface IFeedbackCustomFieldDefinition {
    FeedbackCustomFieldType Type { get; }
    string Alias { get; }
    string Name { get; }
    bool Required { get; }
    int? TextMaxLength { get; }
}