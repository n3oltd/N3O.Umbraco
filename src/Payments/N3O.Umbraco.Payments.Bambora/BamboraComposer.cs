using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using N3O.Umbraco.Composing;
using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Payments.Bambora.Client;
using N3O.Umbraco.Payments.Bambora.Content;
using N3O.Umbraco.Payments.Bambora.Models;
using Refit;
using Umbraco.Cms.Core.DependencyInjection;

namespace N3O.Umbraco.Payments.Bambora {
    public class BamboraComposer : Composer {
        public override void Compose(IUmbracoBuilder builder) {
            builder.Services.AddOpenApiDocument(BamboraConstants.ApiName);

            builder.Services.AddSingleton<BamboraApiSettings>(serviceProvider => {
                var contentCache = serviceProvider.GetRequiredService<IContentCache>();
                var webHostEnvironment = serviceProvider.GetRequiredService<IWebHostEnvironment>();
                var apiSettings = GetApiSettings(contentCache, webHostEnvironment);

                return apiSettings;
            });
            
            builder.Services.AddTransient<IBamboraClient>(serviceProvider => {
                var apiSettings = serviceProvider.GetRequiredService<BamboraApiSettings>();
                IBamboraClient client = null;

                if (apiSettings != null) {
                    var refitSettings = new RefitSettings();
                    refitSettings.ContentSerializer = new NewtonsoftJsonContentSerializer();

                    refitSettings.HttpMessageHandlerFactory =
                        () => new AuthorizationHandler(apiSettings.MerchantId,
                                                                  apiSettings.Passcode);

                    client = RestService.For<IBamboraClient>("https://api.na.bambora.com/v1", refitSettings);
                }
                
                return client;
            });
        }

        private BamboraApiSettings GetApiSettings(IContentCache contentCache, IHostEnvironment environment) {
            var settings = contentCache.Single<BamboraSettingsContent>();

            if (settings != null) {
                if (environment.IsProduction()) {
                    return new BamboraApiSettings(settings.ProductionMerchantId,
                                                  settings.ProductionPasscode);
                } else {
                    return new BamboraApiSettings(settings.StagingMerchantId,
                                                  settings.StagingPasscode);
                }
            }

            return null;
        }
    }
}