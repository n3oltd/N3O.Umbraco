﻿using N3O.Umbraco.Content;
using N3O.Umbraco.Forex;
using N3O.Umbraco.Giving.Allocations;
using N3O.Umbraco.Giving.Allocations.Lookups;
using N3O.Umbraco.Giving.Cart.Models;
using N3O.Umbraco.Lookups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace N3O.Umbraco.Giving.Cart.Entities;

public partial class Cart {
    public async Task BulkRemoveAsync(IContentLocator contentLocator,
                                      IForexConverter forexConverter,
                                      IPriceCalculator priceCalculator,
                                      ILookups lookups,
                                      GivingType givingType,
                                      IEnumerable<int> allocationIndexes) {
        await ReplaceContentsAsync(contentLocator,
                                   forexConverter,
                                   priceCalculator,
                                   lookups,
                                   givingType,
                                   c => BulkRemoveContents(c, allocationIndexes.ToList()));
    }

    private CartContents BulkRemoveContents(CartContents contents, IReadOnlyList<int> allocationIndexes) {
        if (allocationIndexes.Count > contents.Allocations.Count()) {
            throw new ArgumentOutOfRangeException(nameof(allocationIndexes));
        }

        var newAllocations = contents.Allocations.Where((_, index) => !allocationIndexes.Contains(index));

        return new CartContents(Currency, contents.Type, newAllocations);
    }
}