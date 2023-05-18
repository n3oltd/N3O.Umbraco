using N3O.Umbraco.Attributes;
using N3O.Umbraco.Giving.Lookups;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace N3O.Umbraco.Giving.Models;

public class FeedbackAllocationReq : IFeedbackAllocation {
    [Name("Scheme")]
    public FeedbackScheme Scheme { get; set; }

    [Name("Custom Fields")]
    public IEnumerable<FeedbackCustomFieldReq> CustomFields { get; set; }

    [JsonIgnore]
    IEnumerable<IFeedbackCustomField> IFeedbackAllocation.CustomFields => CustomFields;
}
