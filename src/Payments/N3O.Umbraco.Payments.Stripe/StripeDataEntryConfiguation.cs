using Microsoft.Extensions.Hosting;
using N3O.Umbraco.Content;
using N3O.Umbraco.Payments.Stripe.Extensions;
using N3O.Umbraco.Payments.Stripe.Models;

namespace N3O.Umbraco.Payments.Stripe {
    public class StripeDataEntryConfiguation : PaymentMethodDataEntryConfiguration<StripePaymentMethod, StripeDataEntrySettings> {
        private readonly IHostEnvironment _environment;
        private readonly IContentCache _contentCache;

        public StripeDataEntryConfiguation(IHostEnvironment environment, IContentCache contentCache) {
            _environment = environment;
            _contentCache = contentCache;
        }
        protected override StripeDataEntrySettings GetConfig(StripePaymentMethod paymentMethod) {
            var settings = _contentCache.GetStripeKeys(_environment);

            return new StripeDataEntrySettings(settings.Client);
        }
    }
}