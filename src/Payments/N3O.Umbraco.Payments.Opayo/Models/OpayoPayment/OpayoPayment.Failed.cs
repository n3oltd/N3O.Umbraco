namespace N3O.Umbraco.Payments.Opayo.Models {
    public partial class OpayoPayment {
        public void Failed(string transactionId, int? errorCode, string errorMessage) {
            TransactionId = transactionId;
            OpayoErrorCode = errorCode;
            OpayoErrorMessage = errorMessage;
            IsFailed = true;
        }
    }
}