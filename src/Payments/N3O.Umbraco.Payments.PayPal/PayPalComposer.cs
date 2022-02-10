using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using N3O.Umbraco.Composing;
using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Payments.PayPal.Content;
using N3O.Umbraco.Payments.PayPal.Models;
using Umbraco.Cms.Core.DependencyInjection;

namespace N3O.Umbraco.Payments.PayPal {
    public class PayPalComposer : Composer {
        public override void Compose(IUmbracoBuilder builder) {
            builder.Services.AddOpenApiDocument(PayPalConstants.ApiName);
            
            builder.Services.AddTransient<PayPalKeys>(serviceProvider => {
                var contentCache = serviceProvider.GetRequiredService<IContentCache>();
                var webHostEnvironment = serviceProvider.GetRequiredService<IWebHostEnvironment>();
                var settings = contentCache.Single<PayPalSettingsContent>();

                PayPalKeys payPalKeys = null;

                if (settings != null) {
                    if (webHostEnvironment.IsProduction()) {
                        payPalKeys = new PayPalKeys(settings.ProductionClientId);
                    } else {
                        payPalKeys = new PayPalKeys(settings.StagingClientId);
                    }
                }

                return payPalKeys;
            });
        }
    }
}