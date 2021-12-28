using N3O.Umbraco.Financial;
using N3O.Umbraco.Giving.Pricing.Models;

namespace N3O.Umbraco.Giving.Pricing.Extensions;

public static class PricingExtensions {
    public static Money InCurrentCurrency(this IPricing pricing, IHoldPrice item) {
        return pricing.InCurrentCurrencyAsync(item).GetAwaiter().GetResult();
    }
    
    public static Money InCurrency(this IPricing pricing, IHoldPrice item, Currency currency) {
        return pricing.InCurrencyAsync(item, currency).GetAwaiter().GetResult();
    }
}
