using N3O.Umbraco.Context;
using N3O.Umbraco.Financial;
using NodaTime;
using System;
using System.Threading.Tasks;

namespace N3O.Umbraco.Forex;

public class BaseToQuoteForexConverter {
    private readonly IExchangeRateProvider _exchangeRateProvider;
    private readonly IBaseCurrencyAccessor _baseCurrencyAccessor;
    private Currency _baseCurrency;
    private Currency _quoteCurrency;
    private LocalDate? _date;

    public BaseToQuoteForexConverter(IExchangeRateProvider exchangeRateProvider,
                                     IBaseCurrencyAccessor baseCurrencyAccessor) {
        _exchangeRateProvider = exchangeRateProvider;
        _baseCurrencyAccessor = baseCurrencyAccessor;
    }

    public BaseToQuoteForexConverter ToCurrency(Currency quoteCurrency) {
        _quoteCurrency = quoteCurrency;

        return this;
    }

    public BaseToQuoteForexConverter UsingRateOn(LocalDate date) {
        _date = date;

        return this;
    }

    public async Task<ForexMoney> ConvertAsync(decimal baseAmount) {
        if (_quoteCurrency == null) {
            throw new Exception("Quote currency must be specified");
        }

        _baseCurrency ??= _baseCurrencyAccessor.GetBaseCurrency();

        decimal exchangeRate;

        if (_date == null) {
            exchangeRate = await _exchangeRateProvider.GetLiveRateAsync(_baseCurrency, _quoteCurrency);
        } else {
            exchangeRate = await _exchangeRateProvider.GetHistoricalRateAsync(_date.Value,
                                                                              _baseCurrency,
                                                                              _quoteCurrency);
        }

        var quoteAmount = baseAmount * exchangeRate;

        var baseMoney = new Money(baseAmount, _baseCurrency);
        var quoteMoney = new Money(quoteAmount, _quoteCurrency);

        var forex = new ForexMoney(baseMoney, quoteMoney, exchangeRate);

        return forex;
    }
}
