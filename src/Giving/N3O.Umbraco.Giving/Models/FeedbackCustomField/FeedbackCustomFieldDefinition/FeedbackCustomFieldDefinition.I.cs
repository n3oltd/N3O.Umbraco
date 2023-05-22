using N3O.Umbraco.Giving.Lookups;

namespace N3O.Umbraco.Giving.Models;

public interface IFeedbackCustomFieldDefinition {
    FeedbackCustomFieldType Type { get; }
    string Alias { get; }
    string Name { get; }
    bool Required { get; }
    int? TextMaxLength { get; }
}