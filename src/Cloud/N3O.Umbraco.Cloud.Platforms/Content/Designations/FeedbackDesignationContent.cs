using N3O.Umbraco.Attributes;
using N3O.Umbraco.Content;
using N3O.Umbraco.Giving.Allocations.Lookups;
using N3O.Umbraco.Lookups;

namespace N3O.Umbraco.Cloud.Platforms.Content;

[UmbracoContent(PlatformsConstants.FeedbackDesignation.Alias)]
public class FeedbackDesignationContent : UmbracoContent<FeedbackDesignationContent> {
    public FeedbackScheme GetScheme(ILookups lookups) {
        return GetLookup<FeedbackScheme>(lookups, PlatformsConstants.FeedbackDesignation.Properties.Scheme);
    }
}