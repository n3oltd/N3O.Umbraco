using N3O.Umbraco.Extensions;
using N3O.Umbraco.Giving.Cart.Context;
using System;

namespace N3O.Umbraco.Giving.Cart {
    public class CartIdAccessor : ICartIdAccessor {
        private readonly Lazy<CartCookie> _cartCookie;
        private Guid? _cartId;

        public CartIdAccessor(Lazy<CartCookie> cartCookie) {
            _cartCookie = cartCookie;
        }

        public Guid GetCartId() {
            if (_cartId == null) {
                _cartId = _cartCookie.Value.GetValue().TryParseAs<Guid>();

                if (_cartId == null) {
                    throw new Exception("Could not resolve a valid cart ID");
                }
            }

            return _cartId.Value;
        }
    }
}
