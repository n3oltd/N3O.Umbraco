namespace N3O.Umbraco.Payments.PayPal.Models.PayPalCredential;

public partial class PayPalCredential {
    protected override void ClearErrors() {
        base.ClearErrors();
        
        PayPalErrorCode = null;
        PayPalErrorMessage = null;
    }
}