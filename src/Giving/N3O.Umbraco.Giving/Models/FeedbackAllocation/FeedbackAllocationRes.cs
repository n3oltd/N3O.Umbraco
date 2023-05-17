using N3O.Umbraco.Giving.Lookups;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace N3O.Umbraco.Giving.Models;

public class FeedbackAllocationRes : IFeedbackAllocation {
    public FeedbackScheme Scheme { get; set; }
    public IEnumerable<FeedbackCustomFieldRes> CustomFields { get; set; }

    [JsonIgnore]
    IEnumerable<IFeedbackCustomField> IFeedbackAllocation.CustomFields => CustomFields;
}
