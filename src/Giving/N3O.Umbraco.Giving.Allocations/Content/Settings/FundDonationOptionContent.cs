using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Giving.Allocations.Lookups;
using N3O.Umbraco.Lookups;
using System.Collections.Generic;

namespace N3O.Umbraco.Giving.Allocations.Content;

public class FundDonationOptionContent : UmbracoContent<FundDonationOptionContent> {
    public IEnumerable<PriceHandleElement> DonationPriceHandles => GetNestedAs(x => x.DonationPriceHandles);
    public IEnumerable<PriceHandleElement> RegularGivingPriceHandles => GetNestedAs(x => x.RegularGivingPriceHandles);
    
    public DonationItem GetDonationItem(ILookups lookups) => GetLookup<DonationItem>(lookups, AllocationsConstants.Aliases.DonationItem.ContentType);

    public bool IsValid(ILookups lookups) {
        return GetDonationItem(lookups).HasValue();
    }
}
