using N3O.Umbraco.Content;
using N3O.Umbraco.Financial;

namespace N3O.Umbraco.Context {
    public class BaseCurrencyAccessor : IBaseCurrencyAccessor {
        private readonly IContentCache _contentCache;

        public BaseCurrencyAccessor(IContentCache contentCache) {
            _contentCache = contentCache;
        }

        public Currency GetBaseCurrency() {
            var baseCurrency = _contentCache.Single<Currency>(x => x.IsBaseCurrency);

            return baseCurrency;
        }
    }
}