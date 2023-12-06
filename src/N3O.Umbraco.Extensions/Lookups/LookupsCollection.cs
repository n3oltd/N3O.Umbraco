using N3O.Umbraco.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace N3O.Umbraco.Lookups;

public abstract class LookupsCollection<T> : ILookupsCollection<T> where T : ILookup {
    private bool _loaded;
    private Dictionary<string, T> _idDictionary;
    private Dictionary<string, T> _nameDictionary;
    private IReadOnlyList<T> _all;
    
    public virtual async Task<T> FindByIdAsync(string id) {
        await EnsureLoadedAsync();
        
        _idDictionary.TryGetValue(id, out var lookup);

        return lookup;
    }

    public virtual async Task<T> FindByNameAsync(string name) {
        if (!typeof(T).ImplementsInterface<INamedLookup>()) {
            throw new Exception($"{typeof(T).GetFriendlyName()} does not implement {nameof(INamedLookup)} so cannot be searched by name");
        }
        
        await EnsureLoadedAsync();
        
        _nameDictionary.TryGetValue(name, out var lookup);

        return lookup;
    }
    
    async Task<ILookup> ILookupsCollection.FindByIdAsync(string id) {
        var lookup = await FindByIdAsync(id);

        return lookup;
    }
    
    async Task<ILookup> ILookupsCollection.FindByNameAsync(string name) {
        var lookup = await FindByNameAsync(name);

        return lookup;
    }

    async Task<IReadOnlyList<ILookup>> ILookupsCollection.GetAllAsync() {
        var all = await GetAllAsync();

        return all.Cast<ILookup>().ToList();
    }

    public async Task<IReadOnlyList<T>> GetAllAsync() {
        await EnsureLoadedAsync();

        return _all;
    }
    
    private async Task EnsureLoadedAsync() {
        if (!_loaded) {
            _all = await LoadAllAsync();
            _idDictionary = _all.ToDictionary(x => x.Id,
                                              x => x,
                                              StringComparer.InvariantCultureIgnoreCase);

            if (typeof(T).ImplementsInterface<INamedLookup>()) {
                _nameDictionary = _all.ToDictionary(x => ((INamedLookup) x).Name,
                                                    x => x,
                                                    StringComparer.InvariantCultureIgnoreCase);
            }

            _loaded = true;
        }
    }
    
    protected abstract Task<IReadOnlyList<T>> LoadAllAsync();
}
