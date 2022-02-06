using N3O.Umbraco.Giving.Lookups;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace N3O.Umbraco.Giving.Models {
    public class SponsorshipAllocationRes : ISponsorshipAllocation {
        public SponsorshipBeneficiary Beneficiary { get; set; }
        public SponsorshipScheme Scheme { get; set; }
        public SponsorshipDuration Duration { get; set; }
        public IEnumerable<SponsorshipComponentAllocationRes> Components { get; set; }

        [JsonIgnore]
        IEnumerable<ISponsorshipComponentAllocation> ISponsorshipAllocation.Components => Components;
    }
}