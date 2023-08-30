namespace N3O.Umbraco.Payments.TotalProcessing.Models;

public partial class TotalProcessingPayment {
    public void Declined(string transactionId, string statusCode, string statusDetail) {
        ClearErrors();

        TotalProcessingTransactionId = transactionId;
        TotalProcessingStatusCode = statusCode;
        TotalProcessingStatusDetail = statusDetail;
        
        Declined(statusDetail);
    }
}
