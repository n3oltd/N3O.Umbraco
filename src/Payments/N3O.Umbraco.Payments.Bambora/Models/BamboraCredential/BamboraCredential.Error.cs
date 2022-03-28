namespace N3O.Umbraco.Payments.Bambora.Models {
    public partial class BamboraCredential {
        public void Error(int errorCode, string errorMessage) {
            BamboraErrorCode = errorCode;
            BamboraErrorMessage = errorMessage;
            
            Error(errorMessage);
        }
    }
}