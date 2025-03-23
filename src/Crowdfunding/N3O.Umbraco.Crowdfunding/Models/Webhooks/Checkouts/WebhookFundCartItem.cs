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
}