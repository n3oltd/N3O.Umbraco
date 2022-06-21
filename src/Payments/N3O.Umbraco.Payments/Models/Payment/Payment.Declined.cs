namespace N3O.Umbraco.Payments.Models;

public partial class Payment {
    protected void Declined(string reason) {
        DeclinedAt = Clock.GetCurrentInstant();
        DeclinedReason = reason;
        IsDeclined = true;
        PaidAt = null;
        IsPaid = false;
    }
}
