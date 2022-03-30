namespace N3O.Umbraco.Payments.Bambora.Models {
    public partial class BamboraCredential {
        public void Error(int errorCode, string errorMessage) {
            BamboraErrorCode = errorCode;
            BamboraErrorMessage = errorMessage;
            
            Error(GetDisplayMessage(errorCode, errorMessage));
        }

        private string GetDisplayMessage(int errorCode, string errorMessage) {
            if (errorCode == 17) {
                return "This card is already in use";
            } else {
                return errorMessage;
            }
        }
    }
}