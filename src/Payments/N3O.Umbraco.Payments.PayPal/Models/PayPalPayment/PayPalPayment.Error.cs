namespace N3O.Umbraco.Payments.PayPal.Models;

public partial class PayPalPayment {
    public void Error(string transactionId, string details) {
        PayPalTransactionId = transactionId;

        Error(details);
    }
}
