using N3O.Umbraco.Attributes;
using N3O.Umbraco.Giving.Lookups;

namespace N3O.Umbraco.Giving.Models;

public class FeedbackAllocationReq : IFeedbackAllocation {
    [Name("Scheme")]
    public FeedbackScheme Scheme { get; set; }
}
