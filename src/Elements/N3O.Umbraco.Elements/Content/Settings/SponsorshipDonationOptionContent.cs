using N3O.Umbraco.Content;
using N3O.Umbraco.Elements.Lookups;
using N3O.Umbraco.Extensions;
using System.Linq;

namespace N3O.Umbraco.Elements.Content;

public class SponsorshipDonationOptionContent : UmbracoContent<SponsorshipDonationOptionContent> {
    public SponsorshipScheme Scheme => GetValue(x => x.Scheme);

    public bool IsValid() {
        return Scheme.HasValue();
    }
}
