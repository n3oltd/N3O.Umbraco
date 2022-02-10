namespace N3O.Umbraco.Payments.Models {
    public partial class Payment {
        protected void Paid() {
            DeclinedAt = null;
            DeclinedReason = null;
            IsDeclined = false;
            PaidAt = Clock.GetCurrentInstant();
            IsPaid = true;

            Complete();
        }
    }
}