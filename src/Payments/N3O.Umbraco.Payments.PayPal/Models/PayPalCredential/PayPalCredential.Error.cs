namespace N3O.Umbraco.Payments.PayPal.Models;

public partial class PayPalCredential {
    public void Error(int errorCode, string errorMessage) {
        PayPalErrorCode = errorCode;
        PayPalErrorMessage = errorMessage;

        Error(errorMessage);
    }
}