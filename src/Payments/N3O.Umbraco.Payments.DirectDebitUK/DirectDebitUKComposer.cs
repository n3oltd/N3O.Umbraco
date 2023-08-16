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

        builder.Services.AddSingleton<ValidationApiSettings>(serviceProvider => {
            var contentCache = serviceProvider.GetRequiredService<IContentCache>();
            var webHostEnvironment = serviceProvider.GetRequiredService<IWebHostEnvironment>();
            var apiSettings = GetApiSettings(contentCache, webHostEnvironment);

            return apiSettings;
        });
        
        builder.Services.AddSingleton<ILoqateApiClient>(serviceProvider => {
            var apiSettings = serviceProvider.GetRequiredService<ValidationApiSettings>();
            ILoqateApiClient client = null;

            if (apiSettings != null && apiSettings.LoqateApiKey.HasValue()) {
                var refitSettings = new RefitSettings();
                refitSettings.ContentSerializer = new NewtonsoftJsonContentSerializer();

                refitSettings.HttpMessageHandlerFactory = () => new AuthorizationHandler(apiSettings.LoqateApiKey);

                client = RestService.For<ILoqateApiClient>("https://api.addressy.com/BankAccountValidation/Interactive",
                                                           refitSettings);
            }

            return client;
        });
        
        builder.Services.AddSingleton<IFetchifyApiClient>(serviceProvider => {
            var apiSettings = serviceProvider.GetRequiredService<ValidationApiSettings>();
            IFetchifyApiClient client = null;

            if (apiSettings != null && apiSettings.FetchifyApiKey.HasValue()) {
                var refitSettings = new RefitSettings();
                refitSettings.ContentSerializer = new NewtonsoftJsonContentSerializer();

                refitSettings.HttpMessageHandlerFactory = () => new AuthorizationHandler(apiSettings.FetchifyApiKey);

                client = RestService.For<IFetchifyApiClient>("https://api.craftyclicks.co.uk/bank",
                                                             refitSettings);
            }

            return client;
        });
    }

    private ValidationApiSettings GetApiSettings(IContentCache contentCache, IWebHostEnvironment webHostEnvironment) {
        var settings = contentCache.Single<DirectDebitUKSettingsContent>();

        if (settings != null) {
            if (settings.FetchifyApiKey.HasValue()) {
                return new ValidationApiSettings(settings.FetchifyApiKey, null);
            } else if (settings.LoqateApiKey.HasValue()) {
                return new ValidationApiSettings(null, settings.LoqateApiKey);
            } else {
                return null;
            }
        }

        return null;
    }
}