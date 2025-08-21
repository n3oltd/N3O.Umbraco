using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Giving.Allocations.Lookups;
using N3O.Umbraco.Lookups;

namespace N3O.Umbraco.Giving.Allocations.Content;

public class FeedbackDonationOptionContent : UmbracoContent<FeedbackDonationOptionContent> {
    public FeedbackScheme GetScheme(ILookups lookups) => GetLookup<FeedbackScheme>(lookups, AllocationsConstants.Aliases.FeedbackScheme.ContentType);

    public bool IsValid(ILookups lookups) {
        return GetScheme(lookups).HasValue();
    }
}
