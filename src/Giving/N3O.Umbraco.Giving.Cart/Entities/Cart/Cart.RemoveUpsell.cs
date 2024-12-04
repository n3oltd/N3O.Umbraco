using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Forex;
using N3O.Umbraco.Giving.Allocations;
using N3O.Umbraco.Giving.Allocations.Lookups;
using N3O.Umbraco.Giving.Cart.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace N3O.Umbraco.Giving.Cart.Entities;

public partial class Cart {
    public async Task RemoveUpsellAsync(IContentLocator contentLocator,
                                        IForexConverter forexConverter,
                                        IPriceCalculator priceCalculator,
                                        GivingType givingType,
                                        Guid upsellOfferId) {
        await ReplaceContentsAsync(contentLocator,
                                   forexConverter,
                                   priceCalculator,
                                   givingType,
                                   c => RemoveUpsellByOfferId(c, upsellOfferId));
    }

    private CartContents RemoveUpsellByOfferId(CartContents contents, Guid upsellOfferId) {
        var newAllocations = contents.Allocations.ExceptWhere(x => x.UpsellOfferId == upsellOfferId).ToList();

        return new CartContents(Currency, contents.Type, newAllocations);
    }
}