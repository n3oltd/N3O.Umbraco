using System.Collections.Generic;

namespace N3O.Umbraco.Webhooks.Models;

public class WebhookMoney : Value {
    public WebhookMoney(decimal amount, WebhookCurrency currency) {
        Amount = amount;
        Currency = currency;
    }

    public decimal Amount { get; }
    public WebhookCurrency Currency { get; }

    protected override IEnumerable<object> GetAtomicValues() {
        yield return Amount;
        yield return Currency;
    }
}