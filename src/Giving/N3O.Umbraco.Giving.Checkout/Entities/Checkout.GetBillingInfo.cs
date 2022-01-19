using N3O.Umbraco.Payments.Models;

namespace N3O.Umbraco.Giving.Checkout.Entities {
    public partial class Checkout {
        public BillingInfo GetBillingInfo() {
            return new BillingInfo(Account.Address,
                                   Account.Email,
                                   Account.Name,
                                   Account.Telephone);
        }
    }
}