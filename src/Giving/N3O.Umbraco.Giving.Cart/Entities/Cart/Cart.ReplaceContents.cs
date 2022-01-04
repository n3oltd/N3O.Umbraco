using N3O.Umbraco.Exceptions;
using N3O.Umbraco.Giving.Allocations.Lookups;
using N3O.Umbraco.Giving.Cart.Models;
using System;

namespace N3O.Umbraco.Giving.Cart.Entities {
    public partial class Cart {
        private void ReplaceContents(DonationType donationType, Func<CartContents, CartContents> replace) {
            if (donationType == DonationTypes.Single) {
                Single = replace(Single);
            } else if (donationType == DonationTypes.Regular) {
                Regular = replace(Regular);
            } else {
                throw UnrecognisedValueException.For(donationType);
            }
        }
    }
}