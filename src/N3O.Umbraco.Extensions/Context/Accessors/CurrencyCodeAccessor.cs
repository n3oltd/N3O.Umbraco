using System;

namespace N3O.Umbraco.Context;

public class CurrencyCodeAccessor : ICurrencyCodeAccessor {
    private readonly Lazy<CurrencyCookie> _currencyCookie;
    private string _currencyCode;

    public CurrencyCodeAccessor(Lazy<CurrencyCookie> currencyCookie) {
        _currencyCookie = currencyCookie;
    }

    public string GetCurrencyCode() {
        if (_currencyCode == null) {
            _currencyCode = _currencyCookie.Value.GetValue();
        }

        return _currencyCode;
    }
}
