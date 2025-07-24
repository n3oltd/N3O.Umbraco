using N3O.Umbraco.Attributes;
using N3O.Umbraco.Content;
using N3O.Umbraco.Giving.Allocations.Lookups;
using N3O.Umbraco.Lookups;
using System.Collections.Generic;

namespace N3O.Umbraco.Cloud.Platforms.Content;

[UmbracoContent(PlatformsConstants.FundDesignation.Alias)]
public class FundDesignationContent : UmbracoContent<FundDesignationContent> {
    public IEnumerable<SuggestedAmountElement> OneTimeSuggestedAmounts => GetNestedAs(x => x.OneTimeSuggestedAmounts);
    public IEnumerable<SuggestedAmountElement> RecurringSuggestedAmounts => GetNestedAs(x => x.RecurringSuggestedAmounts);

    public DonationItem GetDonationItem(ILookups lookups) {
        return GetLookup<DonationItem>(lookups, PlatformsConstants.FundDesignation.Properties.DonationItem);
    }
}