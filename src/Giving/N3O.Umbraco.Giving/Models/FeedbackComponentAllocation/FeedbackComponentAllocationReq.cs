using N3O.Umbraco.Attributes;
using N3O.Umbraco.Financial;
using N3O.Umbraco.Giving.Lookups;
using Newtonsoft.Json;

namespace N3O.Umbraco.Giving.Models;

public class FeedbackComponentAllocationReq : IFeedbackComponentAllocation {
    [Name("Component")]
    public FeedbackComponent Component { get; set; }
    
    [Name("Value")]
    public MoneyReq Value { get; set; }

    [JsonIgnore]
    Money IFeedbackComponentAllocation.Value => Value;
}
