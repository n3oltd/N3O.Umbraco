using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using N3O.Umbraco.Composing;
using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Payments.Stripe.Models;
using Stripe;
using Umbraco.Cms.Core.DependencyInjection;

namespace N3O.Umbraco.Payments.Stripe {
    public class StripeComposer : Composer {
        public override void Compose(IUmbracoBuilder builder) {
            builder.Services.AddOpenApiDocument(StripeConstants.ApiName);
            
            builder.Services.AddTransient<StripeApiSettings>(serviceProvider => {
                var contentCache = serviceProvider.GetRequiredService<IContentCache>();
                var webHostEnvironment = serviceProvider.GetRequiredService<IWebHostEnvironment>();
                var apiSettings = GetApiSettings(contentCache, webHostEnvironment);

                return apiSettings;
            });
            
            builder.Services.AddTransient<StripeClient>(serviceProvider => {
                var apiSettings = serviceProvider.GetRequiredService<StripeApiSettings>();

                return new StripeClient(apiSettings.SecretKey);
            });
        }
        
        private static StripeApiSettings GetApiSettings(IContentCache contentCache, IHostEnvironment environment) {
            var settings = contentCache.Single<StripeSettingsContent>();
            StripeApiSettings apiSettings = null;
            
            if (settings != null) {
                if (environment.IsProduction()) {
                    apiSettings = new StripeApiSettings(settings.ProductionClientKey, settings.ProductionSecretKey);
                } else {
                    apiSettings = new StripeApiSettings(settings.SandboxClientKey, settings.SandboxSecretKey);
                }  
            }

            return apiSettings;
        }
    }
}