using N3O.Umbraco.Financial;
using NodaTime;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Forex {
    public interface IExchangeRateCache {
        Task<decimal> GetHistoricalRateAsync(LocalDate date,
                                             Currency baseCurrency,
                                             Currency quoteCurrency,
                                             CancellationToken cancellationToken = default);

        Task<decimal> GetLiveRateAsync(Currency baseCurrency,
                                       Currency quoteCurrency,
                                       CancellationToken cancellationToken = default);
    }
}