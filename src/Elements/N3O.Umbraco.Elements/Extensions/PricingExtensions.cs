using N3O.Umbraco.Elements.Models;
using N3O.Umbraco.Extensions;

namespace N3O.Umbraco.Elements.Extensions;

public static class PricingExtensions {
    public static bool HasPricing(this IPricing pricing) {
        if (pricing.Amount > 0) {
            return true;
        }

        foreach (var rule in pricing.Rules.OrEmpty()) {
            if (rule.Amount > 0) {
                return true;
            }
        }

        return false;
    }
}
