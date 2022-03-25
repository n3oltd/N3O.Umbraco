namespace N3O.Umbraco.Payments.Bambora.Models {
    public partial class BamboraPayment {
        public void Declined(string transactionId, int statusCode, string statusDetail) {
            ClearErrors();
            
            BamboraPaymentId = transactionId;
            BamboraStatusCode = statusCode;
            BamboraStatusDetail = statusDetail;
            
            Declined(statusDetail);
        }
    }
}