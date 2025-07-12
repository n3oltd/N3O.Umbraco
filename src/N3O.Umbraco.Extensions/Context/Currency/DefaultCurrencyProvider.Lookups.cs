using N3O.Umbraco.Financial;
using N3O.Umbraco.Lookups;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Context;

public class LookupsDefaultCurrencyProvider : IDefaultCurrencyProvider {
    private readonly ILookups _lookups;

    public LookupsDefaultCurrencyProvider(ILookups lookups) {
        _lookups = lookups;
    }

    public virtual Task<Currency> GetDefaultCurrencyAsync(CancellationToken cancellationToken = default) {
        var currency = AllCurrencies.Single(x => x.IsBaseCurrency);

        return Task.FromResult(currency);
    }
    
    protected IReadOnlyList<Currency> AllCurrencies => _lookups.GetAll<Currency>();
}