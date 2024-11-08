using N3O.Umbraco.Payments.Lookups;

namespace N3O.Umbraco.Payments.Models;

public partial class PaymentObject {
    protected virtual void ClearErrors() {
        ErrorAt = null;
        ErrorMessage = null;
        ExceptionDetails = null;
        Status = PaymentObjectStatuses.InProgress;
    }
}