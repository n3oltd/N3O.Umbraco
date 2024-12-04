using N3O.Umbraco.Attributes;
using N3O.Umbraco.Financial;
using N3O.Umbraco.Giving.Allocations.Lookups;
using N3O.Umbraco.Giving.Allocations.Models;
using Newtonsoft.Json;

namespace N3O.Umbraco.Giving.Models;

public class SponsorshipComponentAllocationReq : ISponsorshipComponentAllocation {
    [Name("Component")]
    public SponsorshipComponent Component { get; set; }
    
    [Name("Value")]
    public MoneyReq Value { get; set; }

    [JsonIgnore]
    Money ISponsorshipComponentAllocation.Value => Value;
}
