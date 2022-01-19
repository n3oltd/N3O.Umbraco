using N3O.Umbraco.Extensions;
using N3O.Umbraco.Giving.Cart;
using System;

namespace N3O.Umbraco.Giving.Checkout {
    public class CheckoutIdAccessor : ICheckoutIdAccessor {
        private readonly ICartIdAccessor _cartIdAccessor;

        public CheckoutIdAccessor(ICartIdAccessor cartIdAccessor) {
            _cartIdAccessor = cartIdAccessor;
        }

        public Guid GetCheckoutId() {
            var cartId = _cartIdAccessor.GetCartId();
            var checkoutId = cartId.Increment();

            return checkoutId;
        }
    }
}
