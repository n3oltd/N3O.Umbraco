using Stripe;

namespace N3O.Umbraco.Payments.Stripe.Models;

public partial class StripeCredential {
    public void Error(StripeException ex) {
        StripeErrorCode = ex.StripeError.Code;
        StripeErrorMessage = ex.StripeError.Message;
        
        Error(ex.StripeError.Message);
    } 
}
