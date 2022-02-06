using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using N3O.Umbraco.Composing;
using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Payments.Stripe.Models;
using Umbraco.Cms.Core.DependencyInjection;

namespace N3O.Umbraco.Payments.Stripe {
    public class StripeComposer : Composer {
        public override void Compose(IUmbracoBuilder builder) {
            builder.Services.AddOpenApiDocument(StripeConstants.ApiName);

            builder.Services.AddTransient<StripeKeys>(serviceProvider => {
                var contentCache = serviceProvider.GetRequiredService<IContentCache>();
                var webHostEnvironment = serviceProvider.GetRequiredService<IWebHostEnvironment>();
                var settings = contentCache.Single<StripeSettingsContent>();

                StripeKeys stripeKeys;

                if (webHostEnvironment.IsProduction()) {
                    stripeKeys = new StripeKeys(settings.ProductionClientKey, settings.ProductionSecretKey);
                } else {
                    stripeKeys = new StripeKeys(settings.StagingClientKey, settings.StagingSecretKey);
                }

                return stripeKeys;
            });
        }
    }
}