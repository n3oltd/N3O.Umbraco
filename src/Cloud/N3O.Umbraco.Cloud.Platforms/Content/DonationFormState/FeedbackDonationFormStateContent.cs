using N3O.Umbraco.Attributes;
using N3O.Umbraco.Content;
using N3O.Umbraco.Giving.Allocations.Lookups;

namespace N3O.Umbraco.Cloud.Platforms.Content;

[UmbracoContent(PlatformsConstants.DonationFormStates.Feedback)]
public class FeedbackDonationFormStateContent : UmbracoContent<FeedbackDonationFormStateContent> {
    public FeedbackScheme Scheme => GetValue(x => x.Scheme);
}