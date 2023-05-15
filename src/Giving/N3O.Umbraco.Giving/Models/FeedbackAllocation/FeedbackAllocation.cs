using N3O.Umbraco.Giving.Lookups;
using Newtonsoft.Json;

namespace N3O.Umbraco.Giving.Models;

public class FeedbackAllocation : Value, IFeedbackAllocation {
    [JsonConstructor]
    public FeedbackAllocation(FeedbackScheme scheme) {
        Scheme = scheme;
    }

    public FeedbackAllocation(IFeedbackAllocation feedback)
        : this(feedback.Scheme) { }
    
    public FeedbackScheme Scheme { get; }

    public string Summary => Scheme?.Name;
}
