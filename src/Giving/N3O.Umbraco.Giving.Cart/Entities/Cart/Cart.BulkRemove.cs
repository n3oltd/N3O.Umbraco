using N3O.Umbraco.Content;
using N3O.Umbraco.Forex;
using N3O.Umbraco.Giving.Cart.Models;
using N3O.Umbraco.Giving.Lookups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace N3O.Umbraco.Giving.Cart.Entities;

public partial class Cart {
    public async Task BulkRemoveAsync(IContentLocator contentLocator,
                                      IForexConverter forexConverter,
                                      IPriceCalculator priceCalculator,
                                      GivingType givingType,
                                      IReadOnlyList<int> allocationIndexes) {
        await ReplaceContentsAsync(contentLocator,
                                   forexConverter,
                                   priceCalculator,
                                   givingType,
                                   c => BulkRemoveContents(c, allocationIndexes.ToList()));
    }

    private CartContents BulkRemoveContents(CartContents contents, IReadOnlyList<int> allocationIndexes) {
        if (allocationIndexes.Count >= contents.Allocations.Count()) {
            throw new IndexOutOfRangeException($"{nameof(allocationIndexes)} are out of range");
        }

        var allocationsToRemove = contents.Allocations.Where((_, index) => allocationIndexes.Contains(index) == true);
        var newAllocations = contents.Allocations.Except(allocationsToRemove);

        return new CartContents(Currency, contents.Type, newAllocations);
    }
}
