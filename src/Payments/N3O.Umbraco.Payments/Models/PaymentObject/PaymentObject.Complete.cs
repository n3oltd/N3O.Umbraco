using N3O.Umbraco.Payments.Lookups;

namespace N3O.Umbraco.Payments.Models;

public partial class PaymentObject {
    protected void Complete() {
        CompleteAt = Clock.GetCurrentInstant();
        Status = PaymentObjectStatuses.Complete;
    }
}
