using N3O.Umbraco.Giving.Lookups;

namespace N3O.Umbraco.Giving.Models;

public interface IFeedbackAllocation {
    FeedbackScheme Scheme { get; }
}
