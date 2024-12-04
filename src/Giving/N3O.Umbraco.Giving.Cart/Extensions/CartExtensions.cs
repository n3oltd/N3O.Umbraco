using N3O.Umbraco.Exceptions;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Financial;
using N3O.Umbraco.Giving.Allocations.Lookups;
using N3O.Umbraco.Giving.Extensions;
using System;
using System.Linq;

namespace N3O.Umbraco.Giving.Cart.Extensions;

public static class CartExtensions {
    public static bool ContainsUpsell(this Entities.Cart cart,
                                      Guid upsellOfferId,
                                      params GivingType[] givingTypes) {
        if (givingTypes.Contains(GivingTypes.Donation) &&
            cart.Donation.OrEmpty(x => x.Allocations).Any(x => x.UpsellOfferId == upsellOfferId)) {
            return true;
        }
        
        if (givingTypes.Contains(GivingTypes.RegularGiving) &&
            cart.RegularGiving.OrEmpty(x => x.Allocations).Any(x => x.UpsellOfferId == upsellOfferId)) {
            return true;
        }

        return false;
    }
    
    public static bool ContainsUpsells(this Entities.Cart cart, params GivingType[] givingTypes) {
        if (givingTypes.Contains(GivingTypes.Donation) &&
            cart.Donation.OrEmpty(x => x.Allocations).Any(x => x.UpsellOfferId.HasValue())) {
            return true;
        }
        
        if (givingTypes.Contains(GivingTypes.RegularGiving) &&
            cart.RegularGiving.OrEmpty(x => x.Allocations).Any(x => x.UpsellOfferId.HasValue())) {
            return true;
        }

        return false;
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