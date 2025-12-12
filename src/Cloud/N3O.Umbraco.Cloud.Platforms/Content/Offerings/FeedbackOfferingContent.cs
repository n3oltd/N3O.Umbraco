using N3O.Umbraco.Attributes;
using N3O.Umbraco.Content;
using N3O.Umbraco.Giving.Allocations.Lookups;

namespace N3O.Umbraco.Cloud.Platforms.Content;

[UmbracoContent(PlatformsConstants.Offerings.Feedback)]
public class FeedbackOfferingContent : UmbracoContent<FeedbackOfferingContent> {
    public FeedbackScheme Scheme => GetValue(x => x.Scheme);
}