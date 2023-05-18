using N3O.Umbraco.Giving.Lookups;
using Newtonsoft.Json;

namespace N3O.Umbraco.Giving.Models;

public class FeedbackCustomField : IFeedbackCustomField {
    [JsonConstructor]
    public FeedbackCustomField(FeedbackCustomFieldType type, string value) {
        Type = type;
        Value = value;
    }

    public FeedbackCustomField(IFeedbackCustomField customField) : this(customField.Type, customField.Value) { }
    
    public FeedbackCustomFieldType Type { get; }
    public string Value { get; }
}