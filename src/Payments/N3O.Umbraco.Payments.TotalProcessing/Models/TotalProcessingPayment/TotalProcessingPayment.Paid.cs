namespace N3O.Umbraco.Payments.TotalProcessing.Models;

public partial class TotalProcessingPayment {
    public void Paid(string transactionId,
                     string statusCode,
                     string statusDetail,
                     string connectorTxId1,
                     string connectorTxId2,
                     string connectorTxId3) {
        ClearErrors();

        TotalProcessingTransactionId = transactionId;
        TotalProcessingStatusCode = statusCode;
        TotalProcessingStatusDetail = statusDetail;
        TotalProcessingConnectorTxId1 = connectorTxId1;
        TotalProcessingConnectorTxId2 = connectorTxId2;
        TotalProcessingConnectorTxId3 = connectorTxId3;
        
        Paid();
    }
}
