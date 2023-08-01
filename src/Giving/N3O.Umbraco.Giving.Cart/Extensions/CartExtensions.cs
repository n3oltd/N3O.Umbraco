using N3O.Umbraco.Extensions;
using System;
using System.Linq;

namespace N3O.Umbraco.Giving.Cart.Extensions;

public static class CartExtensions {
    public static bool ContainsUpsell(this Entities.Cart cart) {
        return cart.Donation.OrEmpty(x => x.Allocations).Any(x => x.UpsellId.HasValue());
    }

    public static bool ContainsUpsell(this Entities.Cart cart, Guid upsellId = default) {
        return cart.Donation.OrEmpty(x => x.Allocations).Any(x => x.UpsellId == upsellId);
    }
}