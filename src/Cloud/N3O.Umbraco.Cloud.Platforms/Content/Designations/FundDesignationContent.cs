using N3O.Umbraco.Content;
using N3O.Umbraco.Giving.Allocations.Lookups;
using System.Collections.Generic;

namespace N3O.Umbraco.Cloud.Platforms.Content;

public class FundDesignationContent : UmbracoContent<FundDesignationContent> {
    public DonationItem DonationItem => GetValue(x => x.DonationItem);
    public IEnumerable<SuggestedAmountContent> OneTimeSuggestedAmounts => GetNestedAs(x => x.OneTimeSuggestedAmounts);
    public IEnumerable<SuggestedAmountContent> RecurringSuggestedAmounts => GetNestedAs(x => x.RecurringSuggestedAmounts);
}