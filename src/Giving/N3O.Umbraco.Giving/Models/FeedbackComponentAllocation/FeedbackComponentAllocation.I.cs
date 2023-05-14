using N3O.Umbraco.Financial;
using N3O.Umbraco.Giving.Lookups;

namespace N3O.Umbraco.Giving.Models;

public interface IFeedbackComponentAllocation {
    FeedbackComponent Component { get; }
    Money Value { get; }
}
