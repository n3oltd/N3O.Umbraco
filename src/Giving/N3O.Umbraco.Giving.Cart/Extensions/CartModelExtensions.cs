using N3O.Umbraco.Extensions;
using N3O.Umbraco.Giving.Cart.Models;
using System.Linq;

namespace N3O.Umbraco.Giving.Cart.Extensions;

public static class CartModelExtensions {
    public static bool ContainsUpsell(this CartModel cartModel) {
        return cartModel.Donation
                        .OrEmpty(x => x.Allocations)
                        .Any(x => x.Upsell);
    }
}