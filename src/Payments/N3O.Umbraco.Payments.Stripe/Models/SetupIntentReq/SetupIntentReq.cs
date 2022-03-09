using N3O.Umbraco.Attributes;
using N3O.Umbraco.Financial;

namespace N3O.Umbraco.Payments.Stripe.Models {
    public class SetupIntentReq {
        [Name("Payment Method Id")]
        public string PaymentMethodId { get; set; }
    }
}