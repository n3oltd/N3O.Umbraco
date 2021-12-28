using N3O.Umbraco.Constants;
using N3O.Umbraco.Content;
using N3O.Umbraco.Financial;

namespace N3O.Umbraco.Context {
    public class CurrencyCodeAccessor : ICurrencyCodeAccessor {
        private readonly ICookieAccessor _cookieAccessor;
        private readonly IContentCache _contentCache;
        private string _currencyCode;

        public CurrencyCodeAccessor(ICookieAccessor cookieAccessor, IContentCache contentCache) {
            _cookieAccessor = cookieAccessor;
            _contentCache = contentCache;
        }

        public string GetCurrencyCode() {
            if (_currencyCode == null) {
                _currencyCode = _cookieAccessor.GetValue(Cookies.Currency);

                if (_currencyCode == null) {
                    _currencyCode = _contentCache.Single<Currency>(x => x.IsBaseCurrency)?.Name;
                }
            }

            return _currencyCode;
        }
    }
}
