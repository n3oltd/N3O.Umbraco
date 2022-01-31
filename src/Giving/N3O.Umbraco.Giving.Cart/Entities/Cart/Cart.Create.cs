using N3O.Umbraco.Entities;
using N3O.Umbraco.Financial;
using N3O.Umbraco.Giving.Lookups;
using N3O.Umbraco.Giving.Cart.Models;
using System;

namespace N3O.Umbraco.Giving.Cart.Entities {
    public partial class Cart {
        public static Cart Create(Guid cartId, Currency currency) {
            var cart = Entity.Create<Cart>(cartId);
            cart.Currency = currency;
            cart.Donation = CartContents.Create(currency, GivingTypes.Donation);
            cart.RegularGiving = CartContents.Create(currency, GivingTypes.RegularGiving);

            return cart;
        }
    }
}