namespace N3O.Umbraco.Payments.Opayo.Models {
    public partial class OpayoPayment {
        public void Error(string vendorTxCode, string transactionId, int? errorCode, string errorMessage) {
            VendorTxCode = vendorTxCode;
            OpayoTransactionId = transactionId;
            OpayoErrorCode = errorCode;
            OpayoErrorMessage = errorMessage;
            
            Error(errorMessage);
        }
    }
}