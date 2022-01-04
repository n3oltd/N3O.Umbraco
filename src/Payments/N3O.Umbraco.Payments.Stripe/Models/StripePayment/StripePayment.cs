using N3O.Umbraco.Payments.Lookups;
using N3O.Umbraco.Payments.Models;

namespace N3O.Umbraco.Payments.Stripe.Models {
    public partial class StripePayment : Payment {
        public string TransactionId { get; private set; }
        
        public override PaymentMethod Method => StripeConstants.PaymentMethod;
    }
}