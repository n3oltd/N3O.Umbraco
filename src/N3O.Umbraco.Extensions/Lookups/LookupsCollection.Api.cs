using Microsoft.Extensions.Caching.Memory;
using N3O.Umbraco.Utilities;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Lookups;

public abstract class ApiLookupsCollection<T> : LookupsCollection<T> where T : ILookup {
    private static readonly MemoryCache Cache = new(new MemoryCacheOptions());

    protected override async Task<IReadOnlyList<T>> LoadAllAsync(CancellationToken cancellationToken) {
        var items = await Cache.GetOrCreateAsync(CacheKey.Generate<ApiLookupsCollection<T>>(), c => {
            c.AbsoluteExpirationRelativeToNow = CacheDuration;

            return FetchAsync(cancellationToken);
        });

        return items;
    }
    
    protected virtual TimeSpan CacheDuration => TimeSpan.FromMinutes(5);

    protected abstract Task<IReadOnlyList<T>> FetchAsync(CancellationToken cancellationToken);
}
