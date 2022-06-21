using N3O.Umbraco.Payments.Models;

namespace N3O.Umbraco.Payments.Entities;

public interface IBillingInfoAccessor {
    BillingInfo GetBillingInfo();
}
