using N3O.Umbraco.Extensions;
using N3O.Umbraco.Giving.Lookups;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace N3O.Umbraco.Giving.Models;

public class FeedbackAllocation : Value, IFeedbackAllocation {
    [JsonConstructor]
    public FeedbackAllocation(FeedbackScheme scheme,
                              IEnumerable<FeedbackComponentAllocation> components) {
        Scheme = scheme;
        Components = components;
    }

    public FeedbackAllocation(IFeedbackAllocation feedback)
        : this(feedback.Scheme,
               feedback.Components.OrEmpty().Select(x => new FeedbackComponentAllocation(x))) { }
    
    public FeedbackScheme Scheme { get; }
    public IEnumerable<FeedbackComponentAllocation> Components { get; }

    public string Summary => Scheme?.Name;

    [JsonIgnore]
    IEnumerable<IFeedbackComponentAllocation> IFeedbackAllocation.Components => Components;
}
