using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Giving.Lookups;
using System.Collections.Generic;

namespace N3O.Umbraco.Giving.Content {
    public class FundDonationOptionContent : UmbracoContent<FundDonationOptionContent> {
        public DonationItem DonationItem => GetAs(x => x.DonationItem);
        public IEnumerable<PriceHandleElement> DonationPriceHandles => GetNestedAs(x => x.DonationPriceHandles);
        public IEnumerable<PriceHandleElement> RegularGivingPriceHandles => GetNestedAs(x => x.RegularGivingPriceHandles);

        public bool IsValid() {
            return DonationItem.HasValue();
        }
    }
}
