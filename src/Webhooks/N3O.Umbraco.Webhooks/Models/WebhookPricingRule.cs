using System.Collections.Generic;

namespace N3O.Umbraco.Webhooks.Models;

public class WebhookPricingRule : Value {
    public WebhookPricingRule(WebhookPrice price, WebhookFundDimensionValues fundDimensions) {
        Price = price;
        FundDimensions = fundDimensions;
    }

    public WebhookPrice Price { get; }
    public WebhookFundDimensionValues FundDimensions { get; }

    protected override IEnumerable<object> GetAtomicValues() {
        yield return Price;
        yield return FundDimensions;
    }
}