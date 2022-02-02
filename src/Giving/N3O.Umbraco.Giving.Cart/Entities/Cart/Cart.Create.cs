using N3O.Umbraco.Financial;
using System;

namespace N3O.Umbraco.Giving.Cart.Entities {
    public partial class Cart {
        public static Cart Create(Guid cartId, Currency currency) {
            var cart = Create<Cart>(cartId);
            
            cart.Reset(currency);

            return cart;
        }
    }
}