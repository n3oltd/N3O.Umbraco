namespace N3O.Umbraco.Payments.Models {
    public partial class Payment {
        protected void Paid() {
            IsPaid = true;
            PaidAt = Clock.GetCurrentInstant();

            Complete();
        }
    }
}