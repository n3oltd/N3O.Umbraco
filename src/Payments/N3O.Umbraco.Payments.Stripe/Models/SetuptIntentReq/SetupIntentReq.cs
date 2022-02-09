using N3O.Umbraco.Attributes;

namespace N3O.Umbraco.Payments.Stripe.Models {
    public class SetupIntentReq {
        [Name("Payment Method ID")]
        public string PaymentMethodId { get; set; }
        
        [Name("Customer ID")]
        public string CustomerId { get; set; }
    }
}