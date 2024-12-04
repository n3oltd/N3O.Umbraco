using N3O.Umbraco.Attributes;
using N3O.Umbraco.Giving.Allocations.Lookups;
using N3O.Umbraco.Giving.Allocations.Models;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace N3O.Umbraco.Giving.Models;

public class SponsorshipAllocationReq : ISponsorshipAllocation {
    [Name("Beneficiary Reference")]
    public string BeneficiaryReference { get; set; }

    [Name("Scheme")]
    public SponsorshipScheme Scheme { get; set; }

    [Name("Duration")]
    public SponsorshipDuration Duration { get; set; }
    
    [Name("Components")]
    public IEnumerable<SponsorshipComponentAllocationReq> Components { get; set; }

    [JsonIgnore]
    IEnumerable<ISponsorshipComponentAllocation> ISponsorshipAllocation.Components => Components;
}
