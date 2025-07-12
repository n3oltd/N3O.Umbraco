using N3O.Umbraco.Financial;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Forex;

public interface IExchangeRateCache {
    Task<decimal> GetLiveRateAsync(Currency baseCurrency,
                                   Currency quoteCurrency,
                                   CancellationToken cancellationToken = default);
}
