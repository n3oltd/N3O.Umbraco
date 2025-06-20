using N3O.Umbraco.Content;
using N3O.Umbraco.Giving.Allocations.Lookups;

namespace N3O.Umbraco.Cloud.Platforms.Content;

public class FeedbackDesignationContent : UmbracoContent<FeedbackDesignationContent> {
    public FeedbackScheme Scheme => GetValue(x => x.Scheme);
}