using N3O.Umbraco.Payments.Lookups;
using N3O.Umbraco.Payments.Stripe.Models;

namespace N3O.Umbraco.Payments.Stripe {
    public class StripePaymentMethod : PaymentMethod {
        public StripePaymentMethod() : base("stripe", "Stripe", true, typeof(StripePayment), typeof(StripeCredential)) { }
    }
}