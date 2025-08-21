using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Giving.Allocations.Lookups;
using N3O.Umbraco.Lookups;

namespace N3O.Umbraco.Giving.Allocations.Content;

public class SponsorshipDonationOptionContent : UmbracoContent<SponsorshipDonationOptionContent> {
    public SponsorshipScheme GetScheme(ILookups lookups) => GetLookup<SponsorshipScheme>(lookups, "scheme");

    public bool IsValid(ILookups lookups) {
        return GetScheme(lookups).HasValue();
    }
}
