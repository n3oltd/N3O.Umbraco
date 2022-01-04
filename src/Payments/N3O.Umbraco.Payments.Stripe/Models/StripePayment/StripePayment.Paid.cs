namespace N3O.Umbraco.Payments.Stripe.Models {
    public partial class StripePayment {
        public void Paid(string transactionId) {
            TransactionId = transactionId;
        }
    }
}