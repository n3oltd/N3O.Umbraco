namespace N3O.Umbraco.Payments.Opayo.Models {
    public partial class OpayoPayment {
        public void Paid(string vendorTxCode,
                         string transactionId,
                         int statusCode,
                         string statusDetail,
                         string bankAuthorisationCode,
                         long retrievalReference) {
            ClearErrors();

            VendorTxCode = vendorTxCode;
            OpayoTransactionId = transactionId;
            OpayoStatusCode = statusCode;
            OpayoStatusDetail = statusDetail;
            OpayoBankAuthorisationCode = bankAuthorisationCode;
            OpayoRetrievalReference = retrievalReference;
            
            Paid();
        }
    }
}