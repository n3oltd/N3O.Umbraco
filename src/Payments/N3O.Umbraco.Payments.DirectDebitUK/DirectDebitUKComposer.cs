using Microsoft.Extensions.DependencyInjection;
using N3O.Umbraco.Composing;
using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Payments.DirectDebitUK.Clients.Fetchify;
using N3O.Umbraco.Payments.DirectDebitUK.Clients.Loqate;
using N3O.Umbraco.Payments.DirectDebitUK.Content;
using N3O.Umbraco.Utilities;
using Refit;
using Umbraco.Cms.Core.DependencyInjection;

namespace N3O.Umbraco.Payments.DirectDebitUK;

public class DirectDebitUKComposer : Composer {
    public override void Compose(IUmbracoBuilder builder) {
        builder.Services.AddOpenApiDocument(DirectDebitUKConstants.ApiName);

        RegisterFetchify(builder);
        RegisterLoqate(builder);

        builder.Services.AddTransient<IUKBankAccountValidatorFactory, UKBankAccountValidatorFactory>();
        
        RegisterAll(t => t.ImplementsInterface<IUKBankAccountValidator>(),
                    t => builder.Services.AddTransient(typeof(IUKBankAccountValidator), t));
    }

    private void RegisterFetchify(IUmbracoBuilder builder) {
        builder.Services.AddSingleton<IFetchifyApiClient>(serviceProvider => {
            var contentCache = serviceProvider.GetRequiredService<IContentCache>();
            var settings = contentCache.Single<DirectDebitUKSettingsContent>();
            var apiKey = settings?.FetchifyApiKey;
            
            IFetchifyApiClient client = null;

            if (apiKey.HasValue()) {
                var refitSettings = new RefitSettings();
                refitSettings.ContentSerializer = new NewtonsoftJsonContentSerializer();

                refitSettings.HttpMessageHandlerFactory = () => new AppendApiKeyToQueryStringHandler("key", apiKey);
                
                client = RestService.For<IFetchifyApiClient>("https://api.craftyclicks.co.uk/bank", refitSettings);
            }

            return client;
        });
    }
    
    private void RegisterLoqate(IUmbracoBuilder builder) {
        builder.Services.AddSingleton<ILoqateApiClient>(serviceProvider => {
            var contentCache = serviceProvider.GetRequiredService<IContentCache>();
            var settings = contentCache.Single<DirectDebitUKSettingsContent>();
            var apiKey = settings?.LoqateApiKey;
            
            ILoqateApiClient client = null;

            if (apiKey.HasValue()) {
                var refitSettings = new RefitSettings();
                refitSettings.ContentSerializer = new NewtonsoftJsonContentSerializer();

                refitSettings.HttpMessageHandlerFactory = () => new AppendApiKeyToQueryStringHandler("key", apiKey);
                
                client = RestService.For<ILoqateApiClient>("https://api.addressy.com/BankAccountValidation/Interactive",
                                                           refitSettings);
            }

            return client;
        });
    }
}