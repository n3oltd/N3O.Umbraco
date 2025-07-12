using N3O.Umbraco.Cloud;
using N3O.Umbraco.Cloud.Extensions;
using N3O.Umbraco.Cloud.Lookups;
using N3O.Umbraco.Cloud.Models;
using N3O.Umbraco.Financial;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Forex.Cloud;

public class CloudExchangeRateProvider : IExchangeRateProvider {
    private readonly ICdnClient _cdnClient;

    public CloudExchangeRateProvider(ICdnClient cdnClient) {
        _cdnClient = cdnClient;
    }

    public async Task<decimal> GetLiveRateAsync(Currency fromCurrency,
                                                Currency toCurrency,
                                                CancellationToken cancellationToken = default) {
        if (fromCurrency == toCurrency) {
            return 1m;
        }
        
        var publishedCurrencies = await _cdnClient.DownloadSubscriptionContentAsync<PublishedCurrencies>(SubscriptionFiles.Currencies,
                                                                                                         JsonSerializers.JsonProvider,
                                                                                                         cancellationToken);

        var publishedFromCurrency = publishedCurrencies.FindByCurrency(fromCurrency, true);
        var publishedToCurrency = publishedCurrencies.FindByCurrency(toCurrency, true);

        var rate = publishedToCurrency.Rate / publishedFromCurrency.Rate;

        return rate;
    }
}
