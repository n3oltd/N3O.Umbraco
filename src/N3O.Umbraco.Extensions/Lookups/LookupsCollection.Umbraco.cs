using N3O.Umbraco.Content;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace N3O.Umbraco.Lookups;

public class UmbracoLookupsCollection<T> : LookupsCollection<T> where T : LookupContent<T> {
    private readonly IReadOnlyList<T> _items;

    public UmbracoLookupsCollection(IContentCache contentCache) {
        _items = contentCache.All<T>().OrderBy(x => x.Content().SortOrder).ToList();
    }

    public override Task<IReadOnlyList<T>> GetAllAsync() {
        return Task.FromResult(_items);
    }
}
