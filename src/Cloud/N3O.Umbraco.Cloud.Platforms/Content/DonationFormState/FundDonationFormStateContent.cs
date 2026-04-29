using N3O.Umbraco.Attributes;
using N3O.Umbraco.Content;
using N3O.Umbraco.Giving.Allocations.Lookups;
using System.Collections.Generic;

namespace N3O.Umbraco.Cloud.Platforms.Content;

[UmbracoContent(PlatformsConstants.DonationFormStates.Fund)]
public class FundDonationFormStateContent : UmbracoContent<FundDonationFormStateContent> {
    public DonationItem DonationItem => GetValue(x => x.DonationItem);
    public IEnumerable<SuggestedAmountElement> OneTimeSuggestedAmounts => GetNestedAs(x => x.OneTimeSuggestedAmounts);
    public IEnumerable<SuggestedAmountElement> RecurringSuggestedAmounts => GetNestedAs(x => x.RecurringSuggestedAmounts);
}