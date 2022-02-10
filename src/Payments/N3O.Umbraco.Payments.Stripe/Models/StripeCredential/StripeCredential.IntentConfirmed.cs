using Stripe;

namespace N3O.Umbraco.Payments.Stripe.Models {
    public partial class StripeCredential {
        public void IntentConfirmed(SetupIntent setupIntent) {
            IntentUpdated(setupIntent);
        }
    }
}