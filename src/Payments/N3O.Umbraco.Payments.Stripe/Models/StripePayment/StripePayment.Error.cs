using Stripe;

namespace N3O.Umbraco.Payments.Stripe.Models;

public partial class StripePayment {
    public void Error(StripeException ex) {
        if (ex.StripeError.Type == "card_error" && ex.StripeError.Code == "card_declined") {
            Declined(ex.StripeError.Code, ex.StripeError.DeclineCode, ex.StripeError.Message);
        } else {
            StripeErrorCode = ex.StripeError.Code;
            StripeErrorMessage = ex.StripeError.Message;
        
            Error(ex.StripeError.Message);
        }
    } 
}
