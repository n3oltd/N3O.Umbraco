using Stripe;

namespace N3O.Umbraco.Payments.Stripe.Models {
    public partial class StripeCredential {
        public void Confirm(SetupIntent setupIntent) {
            StripePaymentMethodId = setupIntent.PaymentMethodId;
            
            IntentUpdated(setupIntent);
        }
    }
}