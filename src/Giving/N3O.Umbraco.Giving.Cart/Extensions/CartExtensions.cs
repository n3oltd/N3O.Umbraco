using N3O.Umbraco.Exceptions;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Financial;
using N3O.Umbraco.Giving.Extensions;
using N3O.Umbraco.Giving.Lookups;
using System;
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
            return cart.Donation.Allocations.GetTotalExcludingUpsells(cart.Currency);
        } else if (givingType == GivingTypes.RegularGiving) {
            return cart.RegularGiving.Allocations.GetTotalExcludingUpsells(cart.Currency);
        } else {
            throw UnrecognisedValueException.For(givingType);
        }
    }
}