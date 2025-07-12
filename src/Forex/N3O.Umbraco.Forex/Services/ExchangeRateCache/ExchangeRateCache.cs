using N3O.Umbraco.Entities;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Financial;
using N3O.Umbraco.Utilities;
using NodaTime;
using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Extensions;

namespace N3O.Umbraco.Forex;

public class ExchangeRateCache : IExchangeRateCache {
    private readonly ConcurrentDictionary<string, decimal> _cache = new();
    private readonly IRepository<CachedExchangeRate> _repository;
    private readonly IClock _clock;
    private readonly IExchangeRateProvider _exchangeRateProvider;

    public ExchangeRateCache(IRepository<CachedExchangeRate> repository,
                             IClock clock,
                             IExchangeRateProvider exchangeRateProvider) {
        _repository = repository;
        _clock = clock;
        _exchangeRateProvider = exchangeRateProvider;
    }

    public async Task<decimal> GetLiveRateAsync(Currency baseCurrency,
                                                Currency quoteCurrency,
                                                CancellationToken cancellationToken = default) {
        var date = _clock.GetCurrentInstant().InUtc().Date;
        
        var rate = await GetOrCreateAsync(date,
                                          baseCurrency,
                                          quoteCurrency,
                                          x => x.GetLiveRateAsync(baseCurrency, quoteCurrency, cancellationToken));

        return rate;
    }
    
    private async Task<decimal> GetOrCreateAsync(LocalDate date,
                                                 Currency baseCurrency,
                                                 Currency quoteCurrency,
                                                 Func<IExchangeRateProvider, Task<decimal>> getRateAsync) {
        var key = $"{date.ToYearMonthDayString()}_{baseCurrency.Id}_{quoteCurrency.Id}";
        var cacheKey = CacheKey.Generate<ExchangeRateCache>(key);

        var result = await _cache.GetOrAddAtomicAsync(cacheKey, async () => {
            var id = key.ToGuid();

            var cachedExchangeRate = await _repository.GetAsync(id);

            if (cachedExchangeRate == null) {
                var rate = await getRateAsync(_exchangeRateProvider);
                
                cachedExchangeRate = await CreateCachedExchangeRateAsync(id,
                                                                         date,
                                                                         baseCurrency,
                                                                         quoteCurrency,
                                                                         rate);
            }

            return cachedExchangeRate.Rate;
        });

        return result;
    }


    private async Task<CachedExchangeRate> CreateCachedExchangeRateAsync(EntityId id,
                                                                         LocalDate date,
                                                                         Currency baseCurrency,
                                                                         Currency quoteCurrency,
                                                                         decimal rate) {
        var cachedExchangeRate = CachedExchangeRate.Create(id, date, baseCurrency, quoteCurrency, rate);

        await _repository.InsertAsync(cachedExchangeRate);

        return cachedExchangeRate;
    }
}
