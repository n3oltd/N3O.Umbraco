using N3O.Umbraco.Content;
using N3O.Umbraco.Financial;
using N3O.Umbraco.Forex.Currencylayer.Content;
using NodaTime;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Forex.Currencylayer {
    public class CurrencylayerExchangeRateProvider : IExchangeRateProvider {
        private readonly ICurrencylayerApiClient _apiClient;
        private readonly IContentCache _contentCache;

        public CurrencylayerExchangeRateProvider(ICurrencylayerApiClient apiClient, IContentCache contentCache) {
            _apiClient = apiClient;
            _contentCache = contentCache;
        }

        public async Task<decimal> GetLiveRateAsync(Currency baseCurrency,
                                                    Currency quoteCurrency,
                                                    CancellationToken cancellationToken = default) {
            if (baseCurrency == quoteCurrency) {
                return 1m;
            }
        
            var req = new LiveRateRequest(baseCurrency, quoteCurrency);
            var res = await _apiClient.GetLiveRateAsync(req);

            return AdjustRate(res.Rate);
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

            return AdjustRate(res.Rate);
        }

        private decimal AdjustRate(decimal value) {
            var settings = _contentCache.Single<CurrencylayerSettingsContent>();
            var rateAdjustment = settings.MarketRateAdjustment;

            if (rateAdjustment < 0) {
                rateAdjustment = 0m;
            } else if (rateAdjustment > 100) {
                rateAdjustment = 100m;
            }
            
            var markerRatePercentage = 100m - rateAdjustment;

            return value * (markerRatePercentage / 100m);
        }
    }
}
