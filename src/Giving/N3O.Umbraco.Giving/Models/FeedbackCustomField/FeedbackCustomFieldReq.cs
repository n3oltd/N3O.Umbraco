using N3O.Umbraco.Giving.Lookups;

namespace N3O.Umbraco.Giving.Models;

public class FeedbackCustomFieldReq : IFeedbackCustomField {
    public FeedbackCustomFieldType Type { get; set; }
    public string Value { get; set;}
}
