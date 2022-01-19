using System;

namespace N3O.Umbraco.Giving.Checkout {
    public interface ICheckoutIdAccessor {
        Guid GetCheckoutId();
    }
}
