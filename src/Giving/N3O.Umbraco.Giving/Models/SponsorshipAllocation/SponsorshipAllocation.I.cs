using N3O.Umbraco.Giving.Lookups;
using System.Collections.Generic;

namespace N3O.Umbraco.Giving.Models;

public interface ISponsorshipAllocation {
    string BeneficiaryReference { get; }
    SponsorshipScheme Scheme { get; }
    SponsorshipDuration Duration { get; }
    IEnumerable<ISponsorshipComponentAllocation> Components { get; }
}
