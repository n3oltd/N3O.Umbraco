using N3O.Umbraco.Giving.Allocations.Lookups;
using N3O.Umbraco.Giving.Allocations.Models;
using N3O.Umbraco.Lookups;
using N3O.Umbraco.Webhooks.Models;
using System.Collections.Generic;

namespace N3O.Umbraco.Crowdfunding.Models;

public class WebhookFundCartItem : Value {
    public WebhookFundCartItem(WebhookLookup donationItem) {
        DonationItem = donationItem;
    }

    public WebhookLookup DonationItem { get; }

    protected override IEnumerable<object> GetAtomicValues() {
        yield return DonationItem;
    }

    public FundAllocation ToFundAllocation(ILookups lookups) {
        var donationItem = lookups.FindById<DonationItem>(DonationItem.Id);

        return new FundAllocation(donationItem);
    }
}