namespace N3O.Umbraco.Payments.Opayo.Models;

public partial class OpayoPayment {
    public void Error(string transactionId, int? errorCode, string errorMessage) {
        OpayoTransactionId = transactionId;
        OpayoErrorCode = errorCode;
        OpayoErrorMessage = errorMessage;
        
        Error(errorMessage);
    }
}
