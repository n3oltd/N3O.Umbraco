namespace N3O.Umbraco.Payments.Bambora.Models {
    public partial class BamboraPayment {
        public void Error(string transactionId, int? errorCode, string errorMessage) {
            BamboraPaymentId = transactionId;
            BamboraErrorCode = errorCode;
            BamboraErrorMessage = errorMessage;
            
            Error(errorMessage);
        }
    }
}