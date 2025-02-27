using N3O.Umbraco.Giving.Allocations.Lookups;
using System.Collections.Generic;

namespace N3O.Umbraco.Giving.Allocations.Models;

public interface ISponsorshipAllocation {
    string BeneficiaryReference { get; }
    SponsorshipScheme Scheme { get; }
    SponsorshipDuration Duration { get; }
    IEnumerable<ISponsorshipComponentAllocation> Components { get; }
}
