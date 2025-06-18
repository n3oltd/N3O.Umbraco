using System.Linq;
using System.Threading.Tasks;

namespace N3O.Umbraco.Lookups;

public static class LookupsExtensions {
    public static T FindByIdOrName<T>(this ILookups lookups, string idOrName) where T : ILookup {
        var lookup = lookups.FindById<T>(idOrName);
        
        lookup ??= lookups.FindByName<T>(idOrName).SingleOrDefault();
        
        return lookup;
    }
    
    public static async Task<T> FindByIdOrNameAsync<T>(this ILookups lookups, string idOrName) where T : ILookup {
        var lookup = await lookups.FindByIdAsync<T>(idOrName);
        
        lookup ??= (await lookups.FindByNameAsync<T>(idOrName)).SingleOrDefault();
        
        return lookup;
    }
}