using N3O.Umbraco.Entities;
using N3O.Umbraco.Giving.Cart.Context;
using System;

namespace N3O.Umbraco.Giving.Cart;

public class CartIdAccessor : ICartIdAccessor {
    private readonly Lazy<CartCookie> _cartCookie;
    private RevisionId _cartId;

    public CartIdAccessor(Lazy<CartCookie> cartCookie) {
        _cartCookie = cartCookie;
    }

    public EntityId GetId() {
        return GetRevisionId().Id;
    }
    
    public RevisionId GetRevisionId() {
        if (_cartId == null) {
            _cartId = GetFromCookie();

            if (_cartId == null) {
                _cartCookie.Value.Reset();
                
                _cartId = GetFromCookie();
            }
        }

        return _cartId;
    }

    private RevisionId GetFromCookie() {
        return RevisionId.TryParse(_cartCookie.Value.GetValue());
    }
}
