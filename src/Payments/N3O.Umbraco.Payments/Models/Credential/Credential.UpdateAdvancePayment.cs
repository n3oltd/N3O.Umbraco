namespace N3O.Umbraco.Payments.Models {
    public partial class Credential {
        public void UpdateAdvancePayment(Payment payment) {
            AdvancePayment = payment;
        }
    }
}