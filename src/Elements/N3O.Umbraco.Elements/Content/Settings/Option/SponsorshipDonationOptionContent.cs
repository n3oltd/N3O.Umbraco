using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Giving.Allocations.Lookups;

namespace N3O.Umbraco.Elements.Content;

public class SponsorshipDonationOptionContent : UmbracoContent<SponsorshipDonationOptionContent> {
    public SponsorshipScheme Scheme => GetValue(x => x.Scheme);

    public bool IsValid() {
        return Scheme.HasValue();
    }
}
