using N3O.Umbraco.Extensions;
using N3O.Umbraco.Giving.Lookups;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace N3O.Umbraco.Giving.Models {
    public class SponsorshipAllocation : Value, ISponsorshipAllocation {
        [JsonConstructor]
        public SponsorshipAllocation(SponsorshipBeneficiary beneficiary,
                                     SponsorshipScheme scheme,
                                     SponsorshipDuration duration,
                                     IEnumerable<ISponsorshipComponentAllocation> components) {
            Beneficiary = beneficiary;
            Scheme = scheme;
            Duration = duration;
            Components = components;
        }

        public SponsorshipAllocation(ISponsorshipAllocation sponsorship)
            : this(sponsorship.Beneficiary,
                   sponsorship.Scheme,
                   sponsorship.Duration,
                   sponsorship.Components.OrEmpty().Select(x => new SponsorshipComponentAllocation(x))) { }

        public SponsorshipBeneficiary Beneficiary { get; }
        public SponsorshipScheme Scheme { get; }
        public SponsorshipDuration Duration { get; }
        public IEnumerable<ISponsorshipComponentAllocation> Components { get; }

        public string Summary => Scheme?.Name;
    }
}