using N3O.Umbraco.Content;
using N3O.Umbraco.Exceptions;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Forex;
using N3O.Umbraco.Giving.Allocations;
using N3O.Umbraco.Giving.Allocations.Content;
using N3O.Umbraco.Giving.Allocations.Extensions;
using N3O.Umbraco.Giving.Allocations.Lookups;
using N3O.Umbraco.Giving.Allocations.Models;
using N3O.Umbraco.Giving.Cart.Models;
using N3O.Umbraco.Lookups;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace N3O.Umbraco.Giving.Cart.Entities;

public partial class Cart {
    private async Task ReplaceContentsAsync(IContentLocator contentLocator,
                                            IForexConverter forexConverter,
                                            IPriceCalculator priceCalculator,
                                            ILookups lookups,
                                            GivingType givingType,
                                            Func<CartContents, CartContents> replace) {
        if (givingType == GivingTypes.Donation) {
            Donation = await UpdateUpsellAmountsAsync(contentLocator,
                                                      forexConverter,
                                                      priceCalculator,
                                                      lookups,
                                                      replace(Donation));
        } else if (givingType == GivingTypes.RegularGiving) {
            RegularGiving = await UpdateUpsellAmountsAsync(contentLocator,
                                                           forexConverter,
                                                           priceCalculator,
                                                           lookups,
                                                           replace(RegularGiving));
        } else {
            throw UnrecognisedValueException.For(givingType);
        }
    }

    private async Task<CartContents> UpdateUpsellAmountsAsync(IContentLocator contentLocator,
                                                              IForexConverter forexConverter,
                                                              IPriceCalculator priceCalculator,
                                                              ILookups lookups,
                                                              CartContents cartContents) {
        if (cartContents.Allocations.None(x => x.UpsellOfferId.HasValue())) {
            return cartContents;
        }

        var allocations = new List<Allocation>();
        
        foreach (var allocation in cartContents.Allocations) {
            if (allocation.UpsellOfferId.HasValue()) {
                var upsellOfferContent = contentLocator.ById<UpsellOfferContent>(allocation.UpsellOfferId.GetValueOrThrow());
                
                var newUpsellAllocation = await upsellOfferContent.ToAllocationAsync(forexConverter,
                                                                                     priceCalculator,
                                                                                     lookups,
                                                                                     Currency,
                                                                                     allocation.Value.Amount,
                                                                                     cartContents.Type,
                                                                                     cartContents.Allocations.GetTotalExcludingUpsells(Currency));

                if (newUpsellAllocation.Value == allocation.Value) {
                    allocations.Add(allocation);
                } else {
                    allocations.Add(newUpsellAllocation);
                }
            } else {
                allocations.Add(allocation);
            }
        }

        return new CartContents(Currency, cartContents.Type, allocations);
    }
}
