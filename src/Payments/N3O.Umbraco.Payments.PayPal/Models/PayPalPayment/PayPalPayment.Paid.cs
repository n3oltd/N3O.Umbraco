namespace N3O.Umbraco.Payments.PayPal.Models {
    public partial class PayPalPayment {
        public void Paid(string transactionId) {
            TransactionId = transactionId;
        }
    }
}