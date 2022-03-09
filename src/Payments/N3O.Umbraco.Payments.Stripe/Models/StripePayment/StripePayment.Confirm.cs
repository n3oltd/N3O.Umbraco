using Stripe;

namespace N3O.Umbraco.Payments.Stripe.Models {
    public partial class StripePayment {
        public void Confirm(PaymentIntent paymentIntent) {
            StripePaymentMethodId = paymentIntent.PaymentMethodId;
            
            IntentUpdated(paymentIntent);
        }
    }
}