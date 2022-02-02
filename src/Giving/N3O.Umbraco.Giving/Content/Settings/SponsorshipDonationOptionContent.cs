using N3O.Umbraco.Content;
using N3O.Umbraco.Giving.Lookups;

namespace N3O.Umbraco.Giving.Content {
    public class SponsorshipDonationOptionContent : UmbracoContent<SponsorshipDonationOptionContent> {
        public SponsorshipScheme Scheme => GetValue(x => x.Scheme);
    }
}
