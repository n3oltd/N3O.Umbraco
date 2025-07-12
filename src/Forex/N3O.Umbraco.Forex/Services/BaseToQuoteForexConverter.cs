using N3O.Umbraco.Context;
using N3O.Umbraco.Financial;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Forex;

public class BaseToQuoteForexConverter {
    private readonly IExchangeRateCache _exchangeRateCache;
    private readonly IBaseCurrencyAccessor _baseCurrencyAccessor;
    private Currency _baseCurrency;
    private Currency _quoteCurrency;

    public BaseToQuoteForexConverter(IExchangeRateCache exchangeRateCache,
                                     IBaseCurrencyAccessor baseCurrencyAccessor) {
        _exchangeRateCache = exchangeRateCache;
        _baseCurrencyAccessor = baseCurrencyAccessor;
    }

    public BaseToQuoteForexConverter ToCurrency(Currency quoteCurrency) {
        _quoteCurrency = quoteCurrency;

        return this;
    }
    
    public ForexMoney Convert(decimal baseAmount) {
        return ConvertAsync(baseAmount).GetAwaiter().GetResult();
    }

    public async Task<ForexMoney> ConvertAsync(decimal baseAmount, CancellationToken cancellationToken = default) {
        if (_quoteCurrency == null) {
            throw new Exception("Quote currency must be specified");
        }
        
        _baseCurrency ??= _baseCurrencyAccessor.GetBaseCurrency();

        var exchangeRate = await _exchangeRateCache.GetLiveRateAsync(_baseCurrency, _quoteCurrency, cancellationToken);

        var quoteAmount = baseAmount * exchangeRate;

        var baseMoney = new Money(baseAmount, _baseCurrency);
        var quoteMoney = new Money(quoteAmount, _quoteCurrency);

        var forex = new ForexMoney(baseMoney, quoteMoney, exchangeRate);

        return forex;
    }
}
