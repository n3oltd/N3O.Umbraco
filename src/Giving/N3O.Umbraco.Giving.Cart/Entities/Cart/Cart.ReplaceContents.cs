using N3O.Umbraco.Exceptions;
using N3O.Umbraco.Giving.Lookups;
using N3O.Umbraco.Giving.Cart.Models;
using System;

namespace N3O.Umbraco.Giving.Cart.Entities;

public partial class Cart {
    private void ReplaceContents(GivingType givingType, Func<CartContents, CartContents> replace) {
        if (givingType == GivingTypes.Donation) {
            Donation = replace(Donation);
        } else if (givingType == GivingTypes.RegularGiving) {
            RegularGiving = replace(RegularGiving);
        } else {
            throw UnrecognisedValueException.For(givingType);
        }
    }
}
