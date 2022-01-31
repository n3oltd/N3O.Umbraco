using N3O.Umbraco.Giving.Pricing.Models;
using N3O.Umbraco.Lookups;

namespace N3O.Umbraco.Giving.Sponsorships.Models {
    public class SponsorshipSchemeComponentRes : NamedLookupRes {
        public PricingRes Pricing { get; set; }
        public bool Mandatory { get; set; }
    }
}