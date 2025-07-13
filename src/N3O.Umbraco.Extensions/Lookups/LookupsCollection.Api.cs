using Microsoft.Extensions.Caching.Memory;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Utilities;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Lookups;

public abstract class ApiLookupsCollection<T> : LookupsCollection<T> where T : ILookup {
    private static readonly MemoryCache Cache = new(new MemoryCacheOptions());

    // This method facilitates migration from content lookups to direct lookups. Content lookups have the
    // ID as the content name (camelized) appended with additional values.
    public override async Task<T> FindByIdAsync(string id, CancellationToken cancellationToken = default) {
        var lookup = await base.FindByIdAsync(id, cancellationToken);
        
        while (lookup == null && id?.Length > 2) {
            id = id.Left(id.Length - 1);
            
            lookup = await base.FindByIdAsync(id, cancellationToken);
        }

        return lookup;
    }
    
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
