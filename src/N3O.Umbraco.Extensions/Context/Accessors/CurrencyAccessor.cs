using N3O.Umbraco.Extensions;
using N3O.Umbraco.Financial;
using N3O.Umbraco.Lookups;

namespace N3O.Umbraco.Context;

public class CurrencyAccessor : ICurrencyAccessor {
    private readonly ILookups _lookups;
    private readonly ICurrencyCodeAccessor _currencyCodeAccessor;
    private Currency _currency;

    public CurrencyAccessor(ILookups lookups, ICurrencyCodeAccessor currencyCodeAccessor) {
        _lookups = lookups;
        _currencyCodeAccessor = currencyCodeAccessor;
    }

    public Currency GetCurrency() {
        if (_currency == null) {
            var currencyCode = _currencyCodeAccessor.GetCurrencyCode();
            var allCurrencies = _lookups.GetAll<Currency>();

            _currency = allCurrencies.FindByCode(currencyCode);
        }

        return _currency;
    }
}
