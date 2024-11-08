namespace N3O.Umbraco.Payments.PayPal.Models;

public partial class PayPalCredential {
    protected override void ClearErrors() {
        base.ClearErrors();
        
        PayPalErrorCode = null;
        PayPalErrorMessage = null;
    }
}