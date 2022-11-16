using N3O.Umbraco.Financial;
using System;

namespace N3O.Umbraco.Giving.Cart.Entities;

public partial class Cart {
    public static Cart Create(Guid id, Currency currency) {
        var cart = Create<Cart>(id);
        
        cart.Reset(currency);

        return cart;
    }
}
