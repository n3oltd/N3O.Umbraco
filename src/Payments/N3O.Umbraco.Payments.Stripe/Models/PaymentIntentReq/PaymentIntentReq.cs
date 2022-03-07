using N3O.Umbraco.Attributes;
using N3O.Umbraco.Financial;

namespace N3O.Umbraco.Payments.Stripe.Models {
    public class PaymentIntentReq {
        [Name("Value")]
        public MoneyReq Value { get; set; }
    }
}