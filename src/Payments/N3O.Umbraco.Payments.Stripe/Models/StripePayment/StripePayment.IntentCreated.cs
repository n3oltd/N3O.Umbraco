using Stripe;

namespace N3O.Umbraco.Payments.Stripe.Models {
    public partial class StripePayment {
        public void IntentCreated(PaymentIntent paymentIntent) {
            StripePaymentIntentId = paymentIntent.Id;
            StripePaymentIntentClientSecret = paymentIntent.ClientSecret;
            StripePaymentMethodId = paymentIntent.PaymentMethodId;
            StripeCustomerId = paymentIntent.CustomerId;
            
            IntentUpdated(paymentIntent);
        }
    }
}