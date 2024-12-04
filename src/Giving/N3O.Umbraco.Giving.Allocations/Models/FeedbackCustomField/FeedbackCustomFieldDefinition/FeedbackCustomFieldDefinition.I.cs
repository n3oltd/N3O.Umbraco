using N3O.Umbraco.Giving.Allocations.Lookups;

namespace N3O.Umbraco.Giving.Allocations.Models;

public interface IFeedbackCustomFieldDefinition {
    FeedbackCustomFieldType Type { get; }
    string Alias { get; }
    string Name { get; }
    bool Required { get; }
    int? TextMaxLength { get; }
}