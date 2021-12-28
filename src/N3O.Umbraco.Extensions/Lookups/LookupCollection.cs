using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace N3O.Umbraco.Lookups;

public abstract class LookupsCollection<T> : ILookupsCollection<T> where T : ILookup {
    public virtual async Task<T> FindByIdAsync(string id) {
        var all = await GetAllAsync();
        var lookup = all.FirstOrDefault(x => x.Id == id);

        return lookup;
    }
    
    async Task<ILookup> ILookupsCollection.FindByIdAsync(string id) {
        var lookup = await FindByIdAsync(id);

        return lookup;
    }

    async Task<IReadOnlyList<ILookup>> ILookupsCollection.GetAllAsync() {
        var all = await GetAllAsync();

        return all.Cast<ILookup>().ToList();
    }

    public abstract Task<IReadOnlyList<T>> GetAllAsync();
}
