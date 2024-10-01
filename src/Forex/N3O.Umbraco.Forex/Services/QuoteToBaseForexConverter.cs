using N3O.Umbraco.Context;
using N3O.Umbraco.Financial;
using NodaTime;
using System;
using System.Threading.Tasks;

namespace N3O.Umbraco.Forex;

public class QuoteToBaseForexConverter {
    private readonly IExchangeRateCache _exchangeRateCache;
    private readonly IBaseCurrencyAccessor _baseCurrencyAccessor;
    private Currency _baseCurrency;
    private Currency _quoteCurrency;
    private LocalDate? _date;

    public QuoteToBaseForexConverter(IExchangeRateCache exchangeRateCache,
                                     IBaseCurrencyAccessor baseCurrencyAccessor) {
        _exchangeRateCache = exchangeRateCache;
        _baseCurrencyAccessor = baseCurrencyAccessor;
    }

    public QuoteToBaseForexConverter FromCurrency(Currency quoteCurrency) {
        _quoteCurrency = quoteCurrency;

        return this;
    }

    public QuoteToBaseForexConverter UsingRateOn(LocalDate date) {
        _date = date;

        return this;
    }

    public async Task<ForexMoney> ConvertAsync(decimal quoteAmount) {
        if (_quoteCurrency == null) {
            throw new Exception("Quote currency must be specified");
        }

        _baseCurrency ??= _baseCurrencyAccessor.GetBaseCurrency();

        decimal exchangeRate;

        if (_date == null) {
            exchangeRate = await _exchangeRateCache.GetLiveRateAsync(_baseCurrency, _quoteCurrency);
        } else {
            exchangeRate = await _exchangeRateCache.GetHistoricalRateAsync(_date.Value,
                                                                           _baseCurrency,
                                                                           _quoteCurrency);
        }

        var baseAmount = quoteAmount / exchangeRate;

        var baseMoney = new Money(baseAmount, _baseCurrency);
        var quoteMoney = new Money(quoteAmount, _quoteCurrency);

        var forex = new ForexMoney(baseMoney, quoteMoney, exchangeRate);

        return forex;
    }
}
