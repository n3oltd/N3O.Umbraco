using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Lookups;

[StaticLookups]
public abstract class StaticLookupsCollection<T> : LookupsCollection<T> where T : ILookup {
    private readonly IReadOnlyList<T> _all;

    protected StaticLookupsCollection() {
        _all = StaticLookups.GetAll<T>(GetType());
    }

    protected override Task<IReadOnlyList<T>> LoadAllAsync(CancellationToken cancellationToken) {
        return Task.FromResult(_all);
    }
}
