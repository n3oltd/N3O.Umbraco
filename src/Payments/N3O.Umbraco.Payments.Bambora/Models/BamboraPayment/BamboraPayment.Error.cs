namespace N3O.Umbraco.Payments.Bambora.Models;

public partial class BamboraPayment {
    public void Error(string paymentId, int errorCode, string errorMessage) {
        BamboraPaymentId = paymentId;
        BamboraErrorCode = errorCode;
        BamboraErrorMessage = errorMessage;
        
        Error(errorMessage);
    }
}
