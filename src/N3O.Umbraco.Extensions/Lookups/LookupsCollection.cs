using N3O.Umbraco.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Lookups;

public abstract class LookupsCollection<T> : ILookupsCollection<T> where T : ILookup {
    private DateTime _nextReloadAt = DateTime.MinValue;
    private Dictionary<string, T> _idDictionary;
    private Dictionary<string, IReadOnlyList<T>> _nameDictionary;
    private IReadOnlyList<T> _all;
    
    public virtual async Task<T> FindByIdAsync(string id, CancellationToken cancellationToken = default) {
        await EnsureLoadedAsync(cancellationToken);
        
        _idDictionary.TryGetValue(id, out var lookup);

        return lookup;
    }

    public virtual async Task<IEnumerable<T>> FindByNameAsync(string name,
                                                              CancellationToken cancellationToken = default) {
        if (!typeof(T).ImplementsInterface<INamedLookup>()) {
            throw new Exception($"{typeof(T).GetFriendlyName()} does not implement {nameof(INamedLookup)} so cannot be searched by name");
        }
        
        await EnsureLoadedAsync(cancellationToken);
        
        _nameDictionary.TryGetValue(name, out var lookups);

        return lookups.OrEmpty();
    }
    
    async Task<ILookup> ILookupsCollection.FindByIdAsync(string id, CancellationToken cancellationToken) {
        var lookup = await FindByIdAsync(id, cancellationToken);

        return lookup;
    }
    
    async Task<IEnumerable<ILookup>> ILookupsCollection.FindByNameAsync(string name,
                                                                        CancellationToken cancellationToken) {
        var lookups = await FindByNameAsync(name, cancellationToken);

        return lookups.Cast<ILookup>().ToList();
    }

    async Task<IReadOnlyList<ILookup>> ILookupsCollection.GetAllAsync(CancellationToken cancellationToken) {
        var all = await GetAllAsync(cancellationToken);

        return all.Cast<ILookup>().ToList();
    }

    public async Task<IReadOnlyList<T>> GetAllAsync(CancellationToken cancellationToken = default) {
        await EnsureLoadedAsync(cancellationToken);

        return _all;
    }
    
    private async Task EnsureLoadedAsync(CancellationToken cancellationToken) {
        if (DateTime.UtcNow > _nextReloadAt) {
            var all = await LoadAllAsync(cancellationToken);

            Reload(all);

            _nextReloadAt = DateTime.UtcNow.Add(ReloadInterval);
        }
    }
    
    protected abstract Task<IReadOnlyList<T>> LoadAllAsync(CancellationToken cancellationToken);

    protected void Reload(IEnumerable<T> all) {
        _all = all.OrEmpty().ToList();
        _idDictionary = _all.ToDictionary(x => x.Id,
                                          x => x,
                                          StringComparer.InvariantCultureIgnoreCase);

        if (typeof(T).ImplementsInterface<INamedLookup>()) {
            _nameDictionary = _all.GroupBy(x => ((INamedLookup) x).Name.ToLowerInvariant())
                                  .ToDictionary(x => x.Key,
                                                x => (IReadOnlyList<T>) x.ToList(),
                                                StringComparer.InvariantCultureIgnoreCase);
        }
    }
    
    protected virtual TimeSpan ReloadInterval => TimeSpan.FromMinutes(5);
}
