using N3O.Umbraco.Entities;

namespace N3O.Umbraco.Giving.Checkout;

public interface ICheckoutIdAccessor {
    EntityId GetId();
}
