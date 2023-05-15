using N3O.Umbraco.Giving.Lookups;

namespace N3O.Umbraco.Giving.Models;

public interface IFeedbackCustomField {
    FeedbackCustomField Type { get; }
    public string Name { get; }
}
