namespace N3O.Umbraco.Payments.TotalProcessing.Models;

public partial class TotalProcessingCredential {
    public void Error(string errorCode, string errorMessage) {
        TotalProcessingErrorCode = errorCode;
        TotalProcessingErrorMessage = errorMessage;
        
        Error(errorMessage);
    }
}
