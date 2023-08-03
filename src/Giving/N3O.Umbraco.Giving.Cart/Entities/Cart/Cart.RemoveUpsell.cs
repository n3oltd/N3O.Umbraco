using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Forex;
using N3O.Umbraco.Giving.Cart.Models;
using N3O.Umbraco.Giving.Lookups;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace N3O.Umbraco.Giving.Cart.Entities;

public partial class Cart {
    public async Task RemoveUpsellAsync(IContentLocator contentLocator,
                                        IForexConverter forexConverter,
                                        IPriceCalculator priceCalculator,
                                        GivingType givingType,
                                        Guid upsellId) {
        await ReplaceContentsAsync(contentLocator,
                                   forexConverter,
                                   priceCalculator,
                                   givingType,
                                   c => RemoveContents(c, upsellId));
    }

    private CartContents RemoveContents(CartContents contents, Guid upsellId) {
        var newAllocations = contents.Allocations
                                     .ExceptWhere(x => x.UpsellId == upsellId)
                                     .ToList();

        return new CartContents(Currency, contents.Type, newAllocations);
    }
}