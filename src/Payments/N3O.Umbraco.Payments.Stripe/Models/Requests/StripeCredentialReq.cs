using N3O.Umbraco.Attributes;

namespace N3O.Umbraco.Payments.Stripe.Models {
    public class StripeCredentialReq {
        [Name("SetupIntent ID")]
        public string SetupIntentId { get; set; }
        
        [Name("Payment Method ID")]
        public string PaymentMethodId { get; set; }
    }
}