using N3O.Umbraco.Giving.Allocations.Lookups;
using N3O.Umbraco.Lookups;
using System.Collections.Generic;

namespace N3O.Umbraco.Giving.Allocations.Models;

public class SponsorshipSchemeRes : NamedLookupRes {
    public IEnumerable<GivingType> AllowedGivingTypes { get; set; }
    public IEnumerable<SponsorshipDuration> AllowedDurations { get; set; }
    public FundDimensionOptionsRes FundDimensionOptions { get; set; }
    public IEnumerable<SponsorshipComponentRes> Components { get; set; }
}
