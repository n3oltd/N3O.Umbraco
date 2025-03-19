using N3O.Umbraco.Extensions;
using N3O.Umbraco.Giving.Checkout.Models;
using N3O.Umbraco.Lookups;
using N3O.Umbraco.Webhooks.Models;
using System.Collections.Generic;
using System.Linq;

namespace N3O.Umbraco.Crowdfunding.Models;

public class WebhookCheckoutDonation : Value {
    public WebhookCheckoutDonation(IEnumerable<WebhookCartItem> cartItems,
                                   WebhookMoney total,
                                   bool isComplete,
                                   bool isRequired) {
        CartItems = cartItems.OrEmpty().ToList();
        Total = total;
        IsComplete = isComplete;
        IsRequired = isRequired;
    }

    public IEnumerable<WebhookCartItem> CartItems { get; }
    public WebhookMoney Total { get; }
    public bool IsComplete { get; }
    public bool IsRequired { get; }

    protected override IEnumerable<object> GetAtomicValues() {
        yield return CartItems;
        yield return Total;
        yield return IsComplete;
        yield return IsRequired;
    }

    public DonationCheckout ToDonationCheckout(ILookups lookups) {
        var allocations = CartItems.Select(x => x.ToAllocation(lookups));

        return new DonationCheckout(allocations, null, Total.ToMoney(lookups));
    }
}