using N3O.Umbraco.Giving.Lookups;

namespace N3O.Umbraco.Giving.Models;

public interface IFeedbackCustomField {
    FeedbackCustomFieldType Type { get; }
    public string Value { get; }
}