using System.Collections.Generic;

namespace N3O.Umbraco.Webhooks.Models;

public class WebhookForexMoney : Value {
    public WebhookForexMoney(WebhookMoney @base, WebhookMoney quote, decimal exchangeRate) {
        Base = @base;
        Quote = quote;
        ExchangeRate = exchangeRate;
    }

    public WebhookMoney Base { get; }
    public WebhookMoney Quote { get; }
    public decimal ExchangeRate { get; }

    protected override IEnumerable<object> GetAtomicValues() {
        yield return Base;
        yield return Quote;
        yield return ExchangeRate;
    }
}