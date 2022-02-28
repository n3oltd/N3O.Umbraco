using N3O.Umbraco.Content;
using N3O.Umbraco.Financial;
using N3O.Umbraco.Lookups;
using N3O.Umbraco.Payments.Lookups;
using N3O.Umbraco.Payments.Stripe.Models;

namespace N3O.Umbraco.Payments.Stripe {
    public class StripePaymentMethod : PaymentMethod {
        public StripePaymentMethod() : base("stripe", "Stripe", true, typeof(StripePayment), typeof(StripeCredential)) { }

        public override bool IsAvailable(IContentCache contentCache, Country country, Currency currency) {
            var settings = contentCache.Single<StripeSettingsContent>();

            if (settings == null) {
                return false;
            }

            return true;
        }
    }
}