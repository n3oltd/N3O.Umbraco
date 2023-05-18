using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Giving.Lookups;
using System.Collections.Generic;

namespace N3O.Umbraco.Giving.Content;

public class FeedbackDonationOptionContent : UmbracoContent<FeedbackDonationOptionContent> {
    public FeedbackScheme Scheme => GetValue(x => x.Scheme);
    public IEnumerable<PriceHandleElement> DonationPriceHandles => GetNestedAs(x => x.DonationPriceHandles);
    public IEnumerable<PriceHandleElement> RegularGivingPriceHandles => GetNestedAs(x => x.RegularGivingPriceHandles);

    public bool IsValid() {
        return Scheme.HasValue();
    }
}
