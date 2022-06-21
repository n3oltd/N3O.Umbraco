using N3O.Umbraco.Context;

namespace N3O.Umbraco.Forex;

public class ForexConverter : IForexConverter {
    private readonly IBaseCurrencyAccessor _baseCurrencyAccessor;
    private readonly IExchangeRateCache _exchangeRateCache;

    public ForexConverter(IBaseCurrencyAccessor baseCurrencyAccessor, IExchangeRateCache exchangeRateCache) {
        _baseCurrencyAccessor = baseCurrencyAccessor;
        _exchangeRateCache = exchangeRateCache;
    }

    public BaseToQuoteForexConverter BaseToQuote() {
        return new BaseToQuoteForexConverter(_exchangeRateCache, _baseCurrencyAccessor);
    }

    public QuoteToBaseForexConverter QuoteToBase() {
        return new QuoteToBaseForexConverter(_exchangeRateCache, _baseCurrencyAccessor);
    }
}
