using N3O.Umbraco.Payments.Lookups;
using N3O.Umbraco.Payments.Models;

namespace N3O.Umbraco.Giving.Checkout.Entities {
    public partial class Checkout {
        public PaymentObject GetPaymentObject(PaymentObjectType type) {
            return Progress.CurrentStage.GetPaymentObject(this, type);
        }
    }
}