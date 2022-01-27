using N3O.Umbraco.Context;

namespace N3O.Umbraco.Forex {
    public class ForexConverter : IForexConverter {
        private readonly IBaseCurrencyAccessor _baseCurrencyAccessor;
        private readonly ICurrencyAccessor _quoteCurrencyAccessor;
        private readonly IExchangeRateCache _exchangeRateCache;

        public ForexConverter(IBaseCurrencyAccessor baseCurrencyAccessor,
                              ICurrencyAccessor quoteCurrencyAccessor,
                              IExchangeRateCache exchangeRateCache) {
            _baseCurrencyAccessor = baseCurrencyAccessor;
            _quoteCurrencyAccessor = quoteCurrencyAccessor;
            _exchangeRateCache = exchangeRateCache;
        }

        public BaseToQuoteForexConverter BaseToQuote() {
            return new BaseToQuoteForexConverter(_exchangeRateCache,
                                                 _baseCurrencyAccessor,
                                                 _quoteCurrencyAccessor);
        }

        public QuoteToBaseForexConverter QuoteToBase() {
            return new QuoteToBaseForexConverter(_exchangeRateCache, _baseCurrencyAccessor, _quoteCurrencyAccessor);
        }
    }
}