using N3O.Umbraco.Financial;
using N3O.Umbraco.Giving.Lookups;
using Newtonsoft.Json;

namespace N3O.Umbraco.Giving.Models;

public class FeedbackComponentAllocation : Value, IFeedbackComponentAllocation {
    [JsonConstructor]
    public FeedbackComponentAllocation(FeedbackComponent component, Money value) {
        Component = component;
        Value = value;
    }

    public FeedbackComponentAllocation(IFeedbackComponentAllocation componentAllocation)
        : this(componentAllocation.Component, componentAllocation.Value) { }

    public FeedbackComponent Component { get; }
    public Money Value { get; }
}
