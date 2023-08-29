namespace N3O.Umbraco.Payments.TotalProcessing.Models;

public partial class TotalProcessingPayment {
    public void Error(string transactionId, string errorCode, string errorMessage) {
        TotalProcessingTransactionId = transactionId;
        TotalProcessingErrorCode = errorCode;
        TotalProcessingErrorMessage = errorMessage;
        
        Error(errorMessage);
    }
}
