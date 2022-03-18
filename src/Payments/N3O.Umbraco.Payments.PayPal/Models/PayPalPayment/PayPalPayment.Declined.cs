namespace N3O.Umbraco.Payments.PayPal.Models {
    public partial class PayPalPayment {
        public void Declined(string transactionId, string email, string statusDetail) {
            PayPalEmail = email;
            PayPalTransactionId = transactionId;

            Declined(statusDetail);
        }
    }
}