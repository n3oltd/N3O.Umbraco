using N3O.Umbraco.Payments.Lookups;

namespace N3O.Umbraco.Payments.Models {
    public partial class PaymentObject {
        protected virtual void ClearErrors() {
            Status = PaymentObjectStatuses.InProgress;
        }
    }
}