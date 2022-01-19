using N3O.Umbraco.Entities;
using N3O.Umbraco.Financial;
using N3O.Umbraco.Giving.Allocations.Lookups;
using N3O.Umbraco.Giving.Cart.Models;
using System;

namespace N3O.Umbraco.Giving.Cart.Entities {
    public partial class Cart {
        public static Cart Create(Guid cartId, Currency currency) {
            var cart = Entity.Create<Cart>(cartId);
            cart.Currency = currency;
            cart.Single = CartContents.Create(currency, DonationTypes.Single);
            cart.Regular = CartContents.Create(currency, DonationTypes.Regular);

            return cart;
        }
    }
}