using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using N3O.Umbraco.Composing;
using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Payments.DirectDebitUK.Clients;
using N3O.Umbraco.Payments.DirectDebitUK.Content;
using N3O.Umbraco.Payments.DirectDebitUK.Models;
using Refit;
using Umbraco.Cms.Core.DependencyInjection;

namespace N3O.Umbraco.Payments.DirectDebitUK;

public class DirectDebitUKComposer : Composer {
    public override void Compose(IUmbracoBuilder builder) {
        builder.Services.AddOpenApiDocument(DirectDebitUKConstants.ApiName);
        
        builder.Services.AddSingleton<LoqateApiSettings>(serviceProvider => {
            var contentCache = serviceProvider.GetRequiredService<IContentCache>();
            var webHostEnvironment = serviceProvider.GetRequiredService<IWebHostEnvironment>();
            var apiSettings = GetApiSettings(contentCache, webHostEnvironment);

            return apiSettings;
        });
        
        builder.Services.AddSingleton<ILoqateApiClient>(serviceProvider => {
            var apiSettings = serviceProvider.GetRequiredService<LoqateApiSettings>();
            ILoqateApiClient client = null;

            if (apiSettings != null) {
                var refitSettings = new RefitSettings();
                refitSettings.ContentSerializer = new NewtonsoftJsonContentSerializer();

                refitSettings.HttpMessageHandlerFactory = () => new AuthorizationHandler(apiSettings.ApiKey);
                
                client = RestService.For<ILoqateApiClient>("https://api.addressy.com/BankAccountValidation/Interactive",
                                                           refitSettings);
            }

            return client;
        });
    }

    private LoqateApiSettings GetApiSettings(IContentCache contentCache, IWebHostEnvironment webHostEnvironment) {
        var settings = contentCache.Single<DirectDebitUKSettingsContent>();

        if (settings != null) {
            return new LoqateApiSettings(settings.LoqateApiKey);
        }

        return null;
    }
}