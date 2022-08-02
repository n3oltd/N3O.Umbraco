using N3O.Umbraco.Extensions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace N3O.Umbraco.Lookups;

public abstract class LookupsCollection<T> : ILookupsCollection<T> where T : ILookup {
    public virtual async Task<T> FindByIdAsync(string id) {
        var all = await GetAllAsync();
        var lookup = all.FirstOrDefault(x => x.Id.EqualsInvariant(id));

        return lookup;
    }

    public virtual async Task<T> FindByNameAsync(string name) {
        var all = await GetAllAsync();
        var lookup = all.FirstOrDefault(x => x.Name.EqualsInvariant(name));

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

    public abstract Task<IReadOnlyList<T>> GetAllAsync();
}
