using N3O.Umbraco.Giving.Lookups;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace N3O.Umbraco.Giving.Models;

public class FeedbackAllocationRes : IFeedbackAllocation {
    public FeedbackScheme Scheme { get; set; }
    public IEnumerable<FeedbackComponentAllocationRes> Components { get; set; }

    [JsonIgnore]
    IEnumerable<IFeedbackComponentAllocation> IFeedbackAllocation.Components => Components;
}
