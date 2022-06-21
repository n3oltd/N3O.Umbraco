using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using N3O.Umbraco.Composing;
using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Payments.PayPal.Clients;
using N3O.Umbraco.Payments.PayPal.Content;
using N3O.Umbraco.Payments.PayPal.Models;
using Refit;
using Umbraco.Cms.Core.DependencyInjection;

namespace N3O.Umbraco.Payments.PayPal;

public class PayPalComposer : Composer {
    public override void Compose(IUmbracoBuilder builder) {
        builder.Services.AddOpenApiDocument(PayPalConstants.ApiName);
        
        builder.Services.AddSingleton<PayPalApiSettings>(serviceProvider => {
            var contentCache = serviceProvider.GetRequiredService<IContentCache>();
            var webHostEnvironment = serviceProvider.GetRequiredService<IWebHostEnvironment>();
            var apiSettings = GetApiSettings(contentCache, webHostEnvironment);
            
            return apiSettings;
        });
        
        builder.Services.AddTransient<IPayPalClient>(serviceProvider => {
            var apiSettings = serviceProvider.GetRequiredService<PayPalApiSettings>();
            IPayPalClient client = null;

            if (apiSettings != null) {
                var refitSettings = new RefitSettings();
                refitSettings.ContentSerializer = new NewtonsoftJsonContentSerializer();
                refitSettings.HttpMessageHandlerFactory = () => new AuthorizationHandler(apiSettings.ClientId,
                                                                                         apiSettings.AccessToken);

                client = RestService.For<IPayPalClient>(apiSettings.BaseUrl, refitSettings);
            }
            
            return client;
        });
    }
    
    private PayPalApiSettings GetApiSettings(IContentCache contentCache, IHostEnvironment environment) {
        var settings = contentCache.Single<PayPalSettingsContent>();
        
        if (settings != null) {
            if (environment.IsProduction()) {
                return new PayPalApiSettings("https://api-m.paypal.com",
                                             settings.ProductionAccessToken,
                                             settings.ProductionClientId);
            } else {
                return new PayPalApiSettings("https://api-m.sandbox.paypal.com",
                                             settings.StagingAccessToken,
                                             settings.StagingClientId);
            }
        }

        return null;
    }
}
