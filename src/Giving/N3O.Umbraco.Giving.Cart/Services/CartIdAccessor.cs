using N3O.Umbraco.Context;
using N3O.Umbraco.Extensions;
using System;

namespace N3O.Umbraco.Giving.Cart;

public class CartIdAccessor : ICartIdAccessor {
    private readonly ICookieAccessor _cookieAccessor;
    private Guid? _cartId;

    public CartIdAccessor(ICookieAccessor cookieAccessor) {
        _cookieAccessor = cookieAccessor;
    }

    public Guid GetCartId() {
        if (_cartId == null) {
            _cartId = _cookieAccessor.GetValue(CartConstants.Cookie).TryParseAs<Guid>() ?? Guid.NewGuid();
        }

        return _cartId.Value;
    }
}
