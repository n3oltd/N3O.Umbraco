using Microsoft.Extensions.DependencyInjection;
using N3O.Umbraco.Composing;
using N3O.Umbraco.Content;
using N3O.Umbraco.Json;
using Refit;
using Umbraco.Cms.Core.DependencyInjection;

namespace N3O.Umbraco.Forex.Currencylayer;

public class CurrencylayerComposer : Composer {
    public override void Compose(IUmbracoBuilder builder) {
        builder.Services.AddTransient<IExchangeRateProvider, CurrencylayerExchangeRateProvider>();

        builder.Services.AddTransient<ICurrencylayerApiClient>(serviceProvider => {
            var jsonProvider = serviceProvider.GetRequiredService<IJsonProvider>();
            var contentCache = serviceProvider.GetRequiredService<IContentCache>();
            
            var client = RestService.For<ICurrencylayerApiClient>("https://api.currencylayer.com",
                                                                  GetRefitSettings(jsonProvider, contentCache));

            return client;
        });
    }
    
    private RefitSettings GetRefitSettings(IJsonProvider jsonProvider, IContentCache contentCache) {
        var refitSettings = new RefitSettings();

        var jsonSettings = jsonProvider.GetSettings();
        jsonSettings.ContractResolver = new SnakeCasePropertyNamesContractResolver();

        refitSettings.ContentSerializer = new NewtonsoftJsonContentSerializer(jsonSettings);
        refitSettings.HttpMessageHandlerFactory = () => new AppendApiKeyHandler(contentCache);

        return refitSettings;
    }
}
