using N3O.Umbraco.Payments.Models;

namespace N3O.Umbraco.Payments {
    public interface IBillingInfoAccessor {
        BillingInfo GetBillingInfo();
    }
}