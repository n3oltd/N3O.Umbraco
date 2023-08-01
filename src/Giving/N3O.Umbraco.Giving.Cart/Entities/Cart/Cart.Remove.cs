using N3O.Umbraco.Content;
using N3O.Umbraco.Forex;
using N3O.Umbraco.Giving.Cart.Models;
using N3O.Umbraco.Giving.Lookups;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace N3O.Umbraco.Giving.Cart.Entities;

public partial class Cart {
    public async Task RemoveAsync(IContentLocator contentLocator,
                                  IForexConverter forexConverter,
                                  IPriceCalculator priceCalculator,
                                  GivingType givingType,
                                  int allocationIndex) {
        await ReplaceContentsAsync(contentLocator,
                                   forexConverter,
                                   priceCalculator,
                                   givingType,
                                   c => RemoveContents(c, allocationIndex));
    }

    private CartContents RemoveContents(CartContents contents, int allocationIndex) {
        if (allocationIndex < 0 || allocationIndex >= contents.Allocations.Count()) {
            throw new IndexOutOfRangeException($"{nameof(allocationIndex)} is out of range");
        }

        var newAllocations = contents.Allocations.ToList();
        newAllocations.RemoveAt(allocationIndex);

        return new CartContents(Currency, contents.Type, newAllocations);
    }
}
