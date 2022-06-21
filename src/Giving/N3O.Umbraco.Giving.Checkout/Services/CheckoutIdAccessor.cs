using N3O.Umbraco.Entities;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Giving.Cart;

namespace N3O.Umbraco.Giving.Checkout;

public class CheckoutIdAccessor : ICheckoutIdAccessor {
    private readonly ICartIdAccessor _cartIdAccessor;

    public CheckoutIdAccessor(ICartIdAccessor cartIdAccessor) {
        _cartIdAccessor = cartIdAccessor;
    }

    public EntityId GetId() {
        var cartRevisionId = _cartIdAccessor.GetRevisionId();
        var checkoutId = cartRevisionId.Id.Value.Increment(cartRevisionId.Revision);

        return checkoutId;
    }
}
