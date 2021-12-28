using N3O.Umbraco.Financial;
using Refit;

namespace N3O.Umbraco.Forex.Currencylayer {
    public abstract class ApiRequest {
        private readonly Currency _baseCurrency;
        private readonly Currency _quoteCurrency;

        protected ApiRequest(Currency baseCurrency, Currency quoteCurrency) {
            _baseCurrency = baseCurrency;
            _quoteCurrency = quoteCurrency;
        }

        [AliasAs("source")]
        public string BaseCurrencyCode => _baseCurrency.Name;

        [AliasAs("currencies")]
        public string QuoteCurrencyCode => _quoteCurrency.Name;
    }
}
