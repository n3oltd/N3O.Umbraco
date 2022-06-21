namespace N3O.Umbraco.Payments.Stripe.Models;

public partial class StripeCredential {
    protected override void ClearErrors() {
        base.ClearErrors();
        
        StripeErrorCode = null;
        StripeDeclineCode = null;
        StripeErrorMessage = null;
    }
}
