using N3O.Umbraco.Extensions;
using N3O.Umbraco.Webhooks.Models;
using System.Collections.Generic;
using System.Linq;

namespace N3O.Umbraco.Crowdfunding.Models;

public class WebhookCheckoutDonation : Value {
    public WebhookCheckoutDonation(IEnumerable<WebhookCartItem> cartItems,
                                   WebhookFlowPayment payment,
                                   WebhookMoney total) {
        CartItems = cartItems.OrEmpty().ToList();
        Payment = payment;
        Total = total;
    }

    public IEnumerable<WebhookCartItem> CartItems { get; }
    public WebhookFlowPayment Payment { get; }
    public WebhookMoney Total { get; }

    public bool IsComplete => IsRequired && Payment?.IsPaid == true;
    public bool IsRequired => CartItems.HasAny();

    protected override IEnumerable<object> GetAtomicValues() {
        yield return CartItems;
        yield return Payment;
        yield return Total;
    }
}