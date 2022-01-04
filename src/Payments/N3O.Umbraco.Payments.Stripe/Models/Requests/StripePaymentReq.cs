using N3O.Umbraco.Attributes;

namespace N3O.Umbraco.Payments.Stripe.Models {
    public class StripePaymentReq {
        [Name("PaymentIntent ID")]
        public string PaymentIntentId { get; set; }
        
        [Name("Payment Method ID")]
        public string PaymentMethodId { get; set; }
    }
}