using N3O.Umbraco.Financial;
using NodaTime;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Forex.Currencylayer;

public class CurrencylayerExchangeRateProvider : IExchangeRateProvider {
    private readonly ICurrencylayerApiClient _apiClient;

    public CurrencylayerExchangeRateProvider(ICurrencylayerApiClient apiClient) {
        _apiClient = apiClient;
    }

    public async Task<decimal> GetLiveRateAsync(Currency baseCurrency,
                                                Currency quoteCurrency,
                                                CancellationToken cancellationToken = default) {
        if (baseCurrency == quoteCurrency) {
            return 1m;
        }
        
        var req = new LiveRateRequest(baseCurrency, quoteCurrency);
        var res = await _apiClient.GetLiveRateAsync(req);

        return res.Rate;
    }

    public async Task<decimal> GetHistoricalRateAsync(LocalDate date,
                                                      Currency baseCurrency,
                                                      Currency quoteCurrency,
                                                      CancellationToken cancellationToken = default) {
        if (baseCurrency == quoteCurrency) {
            return 1m;
        }

        var req = new HistoricalRateRequest(baseCurrency, quoteCurrency, date);
        
        var res = await _apiClient.GetHistoricalRateAsync(req);

        return res.Rate;
    }
}
