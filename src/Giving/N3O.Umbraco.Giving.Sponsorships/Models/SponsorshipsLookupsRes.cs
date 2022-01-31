using N3O.Umbraco.Giving.Sponsorships.Lookups;
using N3O.Umbraco.Lookups;
using System.Collections.Generic;

namespace N3O.Umbraco.Giving.Sponsorships.Models {
    public class SponsorshipsLookupsRes : LookupsRes {
        [FromLookupType(SponsorshipsLookupTypes.SponsorshipDurations)]
        public IEnumerable<SponsorshipDurationRes> SponsorshipDurations { get; set; }
        
        [FromLookupType(SponsorshipsLookupTypes.SponsorshipSchemes)]
        public IEnumerable<SponsorshipSchemeRes> SponsorshipSchemes { get; set; }
    }
}