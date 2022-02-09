using N3O.Umbraco.Attributes;
using N3O.Umbraco.Financial;

namespace N3O.Umbraco.Payments.Stripe.Models {
    public class PaymentIntentReq {
        [Name("Payment Method ID")]
        public string PaymentMethodId { get; set; }
        
        [Name("Customer ID")]
        public string CustomerId { get; set; }
        
        [Name("Value")]
        public MoneyReq Value { get; set; }
    }
}