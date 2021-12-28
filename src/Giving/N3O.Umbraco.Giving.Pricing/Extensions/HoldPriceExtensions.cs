using N3O.Umbraco.Giving.Pricing.Models;

namespace N3O.Umbraco.Giving.Pricing.Extensions;

public static class HoldPriceExtensions {
    public static bool HasPrice(this IHoldPrice holdPrice) {
        return holdPrice.Price > 0;
    }
}