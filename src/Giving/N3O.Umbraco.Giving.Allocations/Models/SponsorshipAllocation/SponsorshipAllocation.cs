using N3O.Umbraco.Giving.Allocations.Lookups;
using Newtonsoft.Json;

namespace N3O.Umbraco.Giving.Allocations.Models; 

public class SponsorshipAllocation : Value, ISponsorshipAllocation {
    [JsonConstructor]
    public SponsorshipAllocation(SponsorshipScheme scheme) {
        Scheme = scheme;
    }

    public SponsorshipAllocation(ISponsorshipAllocation sponsorship) : this(sponsorship.Scheme) { }

    public SponsorshipScheme Scheme { get; }
    
    public string Summary => Scheme?.Name;
}