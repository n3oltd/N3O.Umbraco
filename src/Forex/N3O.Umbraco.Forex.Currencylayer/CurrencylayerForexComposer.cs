using Microsoft.Extensions.DependencyInjection;
using N3O.Umbraco.Composing;
using N3O.Umbraco.Content;
using N3O.Umbraco.Json;
using Refit;
using System;
using System.Net.Http;
using Umbraco.Cms.Core.DependencyInjection;

namespace N3O.Umbraco.Forex.Currencylayer;

public class CurrencylayerForexComposer : Composer {
    public override void Compose(IUmbracoBuilder builder) {
        builder.Services.AddTransient<IExchangeRateProvider, CurrencylayerExchangeRateProvider>();

        builder.Services.AddTransient<ICurrencylayerApiClient>(serviceProvider => {
            var jsonProvider = serviceProvider.GetRequiredService<IJsonProvider>();
            var contentCache = serviceProvider.GetRequiredService<IContentCache>();

            var httpClient = new HttpClient(new AppendApiKeyHandler(new HttpClientHandler(), contentCache));
            httpClient.BaseAddress = new Uri("https://api.currencylayer.com");
        
            var apiClient = RestService.For<ICurrencylayerApiClient>(httpClient,
                                                                     GetRefitSettings(jsonProvider));

            return apiClient;
        });
    }

    private RefitSettings GetRefitSettings(IJsonProvider jsonProvider) {
        var jsonSettings = jsonProvider.GetSettings();
        
        var refitSettings = new RefitSettings();
        jsonSettings.ContractResolver = new SnakeCasePropertyNamesContractResolver();
        refitSettings.ContentSerializer = new NewtonsoftJsonContentSerializer(jsonSettings);

        return refitSettings;
    }
}
