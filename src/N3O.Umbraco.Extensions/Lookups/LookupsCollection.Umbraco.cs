using Humanizer;
using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace N3O.Umbraco.Lookups;

public class UmbracoLookupsCollection<T> : LookupsCollection<T> where T : LookupContent<T> {
    private readonly ConcurrentDictionary<string, ConcurrentDictionary<Guid, int>> _suffixes = new(StringComparer.InvariantCultureIgnoreCase);
    
    private readonly IContentCache _contentCache;
    
    public UmbracoLookupsCollection(IContentCache contentCache) {
        _contentCache = contentCache;
    }

    protected override Task<IReadOnlyList<T>> LoadAllAsync() {
        var items = _contentCache.All<T>().OrderBy(x => x.Content().SortOrder).ToList();

        foreach (var item in items) {
            SetId(item);
        }
        
        return Task.FromResult<IReadOnlyList<T>>(items);
    }
    
    private void SetId(T item) {
        var content = item.Content();
        var id = content.Name;

        if (id == id.ToUpperInvariant()) {
            id = id.ToLowerInvariant();
        }

        id = Regex.Replace(id, "[^0-9a-z-_]", "", RegexOptions.CultureInvariant|RegexOptions.IgnoreCase).Camelize();

        var idSuffixes = _suffixes.GetOrAdd(id, () => new ConcurrentDictionary<Guid, int>());
        var suffix = idSuffixes.GetOrAdd(content.Key, () => idSuffixes.Count);
        
        if (suffix != 0) {
            id += suffix;
        }
        
        item.Id = id;
    }
}
