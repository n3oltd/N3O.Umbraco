using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Giving.Lookups;

namespace N3O.Umbraco.Giving.Content;

public class FeedbackDonationOptionContent : UmbracoContent<FeedbackDonationOptionContent> {
    public FeedbackScheme Scheme => GetValue(x => x.Scheme);

    public bool IsValid() {
        return Scheme.HasValue();
    }
}
