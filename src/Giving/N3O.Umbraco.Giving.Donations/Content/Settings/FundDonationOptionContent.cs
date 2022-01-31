using N3O.Umbraco.Content;
using N3O.Umbraco.Giving.Allocations.Lookups;
using N3O.Umbraco.Giving.Donations.Models;
using System.Collections.Generic;

namespace N3O.Umbraco.Giving.Donations.Content {
    public class FundDonationOptionContent : UmbracoContent<FundDonationOptionContent> {
        public DonationItem DonationItem => GetAs(x => x.DonationItem);
        public IEnumerable<PriceHandleElement> DonationPriceHandles => GetNestedAs(x => x.DonationPriceHandles);
        public IEnumerable<PriceHandleElement> RegularGivingPriceHandles => GetNestedAs(x => x.RegularGivingPriceHandles);
    }
}
