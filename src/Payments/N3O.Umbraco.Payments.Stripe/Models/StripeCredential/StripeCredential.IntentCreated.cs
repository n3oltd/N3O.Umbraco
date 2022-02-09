using Stripe;

namespace N3O.Umbraco.Payments.Stripe.Models {
    public partial class StripeCredential {
        public void IntentCreated(SetupIntent setupIntent) {
            StripeSetupIntentId = setupIntent.Id;
            StripeSetupIntentClientSecret = setupIntent.ClientSecret;
            StripePaymentMethodId = setupIntent.PaymentMethodId;
            StripeCustomerId = setupIntent.CustomerId;
            
            IntentUpdated(setupIntent);
        }
    }
}