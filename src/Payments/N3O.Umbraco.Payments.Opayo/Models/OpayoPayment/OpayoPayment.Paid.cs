namespace N3O.Umbraco.Payments.Opayo.Models {
    public partial class OpayoPayment {
        public void Paid(string transactionId, string bankAuthorisationCode, long retrievalReference) {
            TransactionId = transactionId;
            BankAuthorisationCode = bankAuthorisationCode;
            OpayoRetrievalReference = retrievalReference;
            IsPaid = true;
        }
    }
}