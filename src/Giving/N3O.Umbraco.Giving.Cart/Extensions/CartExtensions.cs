using N3O.Umbraco.Exceptions;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Financial;
using N3O.Umbraco.Giving.Lookups;
using N3O.Umbraco.Giving.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace N3O.Umbraco.Giving.Cart.Extensions;

public static class CartExtensions {
    public static bool ContainsUpsell(this Entities.Cart cart, Guid upsellId) {
        return cart.Donation.OrEmpty(x => x.Allocations).Any(x => x.UpsellId == upsellId);
    }
    
    public static bool ContainsUpsells(this Entities.Cart cart) {
        return cart.Donation.OrEmpty(x => x.Allocations).Any(x => x.UpsellId.HasValue());
    }

    public static Money GetTotalExcludingUpsells(this Entities.Cart cart, GivingType givingType) {
        if (givingType == GivingTypes.Donation) {
            return GetTotalExcludingUpsells(cart.Currency, cart.Donation.Allocations);
        } else if (givingType == GivingTypes.RegularGiving) {
            return GetTotalExcludingUpsells(cart.Currency, cart.RegularGiving.Allocations);
        } else {
            throw UnrecognisedValueException.For(givingType);
        }
    }

    private static Money GetTotalExcludingUpsells(Currency currency, IEnumerable<Allocation> allocations) {
        allocations = allocations.OrEmpty().Where(x => !x.UpsellId.HasValue()).ToList();
        
        if (allocations.Any()) {
            return allocations.Select(x => x.Value).Sum();
        } else {
            return currency.Zero();
        }
    }
}