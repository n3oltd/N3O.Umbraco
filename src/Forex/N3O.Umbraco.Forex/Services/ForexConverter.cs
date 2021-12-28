using N3O.Umbraco.Context;

namespace N3O.Umbraco.Forex {
    public class ForexConverter : IForexConverter {
        private readonly IBaseCurrencyAccessor _baseCurrencyAccessor;
        private readonly IExchangeRateProvider _exchangeRateProvider;

        public ForexConverter(IBaseCurrencyAccessor baseCurrencyAccessor, IExchangeRateProvider exchangeRateProvider) {
            _baseCurrencyAccessor = baseCurrencyAccessor;
            _exchangeRateProvider = exchangeRateProvider;
        }

        public BaseToQuoteForexConverter BaseToQuote() {
            return new BaseToQuoteForexConverter(_exchangeRateProvider, _baseCurrencyAccessor);
        }

        public QuoteToBaseForexConverter QuoteToBase() {
            return new QuoteToBaseForexConverter(_exchangeRateProvider, _baseCurrencyAccessor);
        }
    }
}