using N3O.Umbraco.Content;
using N3O.Umbraco.Giving.Allocations.Lookups;

namespace N3O.Umbraco.Giving.Donations.Content {
    public class SponsorshipDonationOptionContent : UmbracoContent<SponsorshipDonationOptionContent> {
        public SponsorshipScheme Scheme => GetValue(x => x.Scheme);
    }
}
