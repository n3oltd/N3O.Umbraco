using N3O.Umbraco.Payments.Lookups;
using N3O.Umbraco.Payments.Models;

namespace N3O.Umbraco.Payments.Stripe.Models {
    public partial class StripeCredential : Credential {
        public StripeCredential() : base(null) { }
        
        public string StripeMandateId { get; private set; }
        public string StripeCustomerId { get; private set; }
        public string StripeDeclineCode { get; private set; }
        public string StripeErrorCode { get; private set; }
        public string StripeErrorMessage { get; private set; }
        public string StripeSetupIntentId { get; private set; }
        public string StripeSetupIntentClientSecret { get; private set; }
        public string StripePaymentMethodId { get; private set; }

        public bool ActionRequired { get; private set; }
        
        public override PaymentMethod Method => StripeConstants.PaymentMethod;
    }
}