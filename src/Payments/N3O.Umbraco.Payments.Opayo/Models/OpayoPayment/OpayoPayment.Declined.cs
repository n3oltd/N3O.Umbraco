namespace N3O.Umbraco.Payments.Opayo.Models {
    public partial class OpayoPayment {
        public void Declined(string transactionId, int statusCode, string statusDetail) {
            OpayoTransactionId = transactionId;
            OpayoStatusCode = statusCode;
            OpayoStatusDetail = statusDetail;
            
            Declined(statusDetail);
        }
    }
}