using N3O.Umbraco.Payments.Lookups;

namespace N3O.Umbraco.Payments.Models {
    public partial class PaymentObject {
        protected void Error(string message) {
            ErrorAt = Clock.GetCurrentInstant();
            ErrorMessage = message;
            Status = PaymentObjectStatuses.Error;
        }
    }
}