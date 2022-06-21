using N3O.Umbraco.Financial;
using N3O.Umbraco.Giving.Lookups;
using Newtonsoft.Json;

namespace N3O.Umbraco.Giving.Models;

public class SponsorshipComponentAllocationRes : ISponsorshipComponentAllocation {
    public SponsorshipComponent Component { get; set; }
    public MoneyRes Value { get; set; }

    [JsonIgnore]
    Money ISponsorshipComponentAllocation.Value => Value;
}
