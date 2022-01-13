using N3O.Umbraco.Content;
using N3O.Umbraco.Giving.Allocations.Lookups;

namespace N3O.Umbraco.Giving.Donations.Content {
    public class SponsorshipDonationOption : UmbracoContent<SponsorshipDonationOption> {
        public SponsorshipScheme Scheme => GetValue(x => x.Scheme);
    }
}
