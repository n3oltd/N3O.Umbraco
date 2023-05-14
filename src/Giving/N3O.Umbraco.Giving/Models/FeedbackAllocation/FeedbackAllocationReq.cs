using N3O.Umbraco.Attributes;
using N3O.Umbraco.Giving.Lookups;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace N3O.Umbraco.Giving.Models;

public class FeedbackAllocationReq : IFeedbackAllocation {
    [Name("Scheme")]
    public FeedbackScheme Scheme { get; set; }

    [Name("Components")]
    public IEnumerable<FeedbackComponentAllocationReq> Components { get; set; }

    [JsonIgnore]
    IEnumerable<IFeedbackComponentAllocation> IFeedbackAllocation.Components => Components;
}
