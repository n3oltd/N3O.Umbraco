using Microsoft.Extensions.Hosting;
using N3O.Umbraco.Content;
using N3O.Umbraco.Payments.Stripe.Models;

namespace N3O.Umbraco.Payments.Stripe.Extensions {
    public static class ContentCacheExtensions {
        public static StripeKeys GetStripeKeys(this IContentCache contentCache, IHostEnvironment environment) {
            var settings = contentCache.Single<StripeSettingsContent>();
            StripeKeys stripeKeys = null;
            
            if (settings != null) {
                if (environment.IsProduction()) {
                    stripeKeys = new StripeKeys(settings.ProductionClientKey, settings.ProductionSecretKey);
                } else {
                    stripeKeys = new StripeKeys(settings.StagingClientKey, settings.StagingSecretKey);
                }  
            }

            return stripeKeys;
        }
    }
}