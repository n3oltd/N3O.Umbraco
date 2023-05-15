using N3O.Umbraco.Giving.Lookups;

namespace N3O.Umbraco.Giving.Models;

public class FeedbackCustomFieldRes : IFeedbackCustomField {
    public FeedbackCustomField Type { get; set; }
    public string Name { get; set;}
}
