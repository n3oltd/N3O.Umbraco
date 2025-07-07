using N3O.Umbraco.Financial;
using N3O.Umbraco.Lookups;
using System.Linq;

namespace N3O.Umbraco.Context;

public class BaseCurrencyAccessor : IBaseCurrencyAccessor {
    private readonly ILookups _lookups;

    public BaseCurrencyAccessor(ILookups lookups) {
        _lookups = lookups;
    }

    public Currency GetBaseCurrency() {
        var baseCurrency = _lookups.GetAll<Currency>().Single(x => x.IsBaseCurrency);

        return baseCurrency;
    }
}
