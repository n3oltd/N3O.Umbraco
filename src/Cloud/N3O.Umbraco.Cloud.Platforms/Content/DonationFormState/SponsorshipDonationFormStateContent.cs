using N3O.Umbraco.Attributes;
using N3O.Umbraco.Content;
using N3O.Umbraco.Giving.Allocations.Lookups;

namespace N3O.Umbraco.Cloud.Platforms.Content;

[UmbracoContent(PlatformsConstants.DonationFormStates.Sponsorship)]
public class SponsorshipDonationFormStateContent : UmbracoContent<SponsorshipDonationFormStateContent> {
    public SponsorshipScheme Scheme {
        get {
            var value = Content().GetProperty("scheme")?.GetValue(VariationContext?.Culture, VariationContext?.Segment)
                     ?? Content().GetProperty("sponsorshipScheme")?.GetValue(VariationContext?.Culture, VariationContext?.Segment);
            return value as SponsorshipScheme;
        }
    }
}