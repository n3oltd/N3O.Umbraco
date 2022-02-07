using N3O.Umbraco.Payments.Lookups;
using N3O.Umbraco.Payments.Models;

namespace N3O.Umbraco.Payments.Stripe.Models {
    public class StripeCredential : Credential {
        public StripeCredential() : base(null) { }
        
        public override PaymentMethod Method => StripeConstants.PaymentMethod;
    }
}