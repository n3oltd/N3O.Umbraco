using N3O.Umbraco.Extensions;
using N3O.Umbraco.Financial;
using N3O.Umbraco.Forex;
using N3O.Umbraco.Giving.Cart.Models;
using N3O.Umbraco.Giving.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace N3O.Umbraco.Giving.Cart.Extensions;

public static class CartExtensions {
    public static bool ContainsUpsell(this Entities.Cart cart) {
        return cart.Donation.OrEmpty(x => x.Allocations).Any(x => x.UpsellId.HasValue());
    }

    public static bool ContainsUpsell(this Entities.Cart cart, Guid upsellId = default) {
        return cart.Donation.OrEmpty(x => x.Allocations).Any(x => x.UpsellId == upsellId);
    }

    public static async Task<IReadOnlyList<UpsellModel>> GetUpsellsAsync(this Entities.Cart cart,
                                                                         IForexConverter forexConverter,
                                                                         IPriceCalculator priceCalculator,
                                                                         IEnumerable<UpsellContent> upsellContents,
                                                                         Currency currency) {
        if (upsellContents.None()) {
            return null;
        }

        var upsells = new List<UpsellModel>();

        foreach (var upsellContent in upsellContents) {
            var priceOrAmount = await upsellContent.GetPriceOrAmountInCurrencyAsync(forexConverter,
                                                                                    priceCalculator,
                                                                                    currency,
                                                                                    cart.RegularGiving.Total,
                                                                                    cart.Donation.Total);

            upsells.Add(new UpsellModel(upsellContent.Content().Key,
                                        upsellContent.Content().Name,
                                        upsellContent.Description,
                                        priceOrAmount,
                                        upsellContent.PriceHandles));
        }

        return upsells;
    }
}