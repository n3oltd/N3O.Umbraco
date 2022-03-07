using N3O.Umbraco.Exceptions;
using Stripe;

namespace N3O.Umbraco.Payments.Stripe.Models {
    public partial class StripeCredential {
        private void IntentUpdated(SetupIntent setupIntent) {
            ClearErrors();
            
            if (setupIntent.Status == "succeeded") {
                SetUp(setupIntent.MandateId);
            } else if (setupIntent.Status == "requires_action") {
                ActionRequired = true;
            } else if (setupIntent.Status == "requires_payment_method") {
                
            } else {
                throw UnrecognisedValueException.For(setupIntent.Status);
            }
        }
    }
}