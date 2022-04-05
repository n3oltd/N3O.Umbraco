namespace N3O.Umbraco.Payments.Opayo.Models {
    public partial class OpayoPayment {
        public void Declined(string vendorTxCode, string transactionId, int statusCode, string statusDetail) {
            ClearErrors();

            VendorTxCode = vendorTxCode;
            OpayoTransactionId = transactionId;
            OpayoStatusCode = statusCode;
            OpayoStatusDetail = statusDetail;
            
            Declined(statusDetail);
        }
    }
}