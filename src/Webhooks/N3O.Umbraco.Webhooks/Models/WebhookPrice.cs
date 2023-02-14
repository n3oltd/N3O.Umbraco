using System.Collections.Generic;

namespace N3O.Umbraco.Webhooks.Models;

public class WebhookPrice : Value {
    public WebhookPrice(decimal amount, bool locked) {
        Amount = amount;
        Locked = locked;
    }

    public decimal Amount { get; }
    public bool Locked { get; }

    protected override IEnumerable<object> GetAtomicValues() {
        yield return Amount;
        yield return Locked;
    }
}
