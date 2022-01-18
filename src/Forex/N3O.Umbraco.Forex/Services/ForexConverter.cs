using N3O.Umbraco.Context;

namespace N3O.Umbraco.Forex {
    public class ForexConverter : IForexConverter {
        private readonly IBaseCurrencyAccessor _baseCurrencyAccessor;
        private readonly ICurrencyAccessor _quoteCurrencyAccessor;
        private readonly IExchangeRateProvider _exchangeRateProvider;

        public ForexConverter(IBaseCurrencyAccessor baseCurrencyAccessor,
                              ICurrencyAccessor quoteCurrencyAccessor,
                              IExchangeRateProvider exchangeRateProvider) {
            _baseCurrencyAccessor = baseCurrencyAccessor;
            _quoteCurrencyAccessor = quoteCurrencyAccessor;
            _exchangeRateProvider = exchangeRateProvider;
        }

        public BaseToQuoteForexConverter BaseToQuote() {
            return new BaseToQuoteForexConverter(_exchangeRateProvider,
                                                 _baseCurrencyAccessor,
                                                 _quoteCurrencyAccessor);
        }

        public QuoteToBaseForexConverter QuoteToBase() {
            return new QuoteToBaseForexConverter(_exchangeRateProvider, _baseCurrencyAccessor, _quoteCurrencyAccessor);
        }
    }
}