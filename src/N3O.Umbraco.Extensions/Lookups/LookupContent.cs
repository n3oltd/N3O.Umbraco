using Humanizer;
using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace N3O.Umbraco.Lookups;

public abstract class LookupContent<T> : UmbracoContent<T>, INamedLookup {
    private static readonly ConcurrentDictionary<string, ConcurrentDictionary<Guid, int>> Suffixes = new(StringComparer.InvariantCultureIgnoreCase);
    
    public virtual string Id => GetId();
    public virtual string Name => Content().Name;

    public IEnumerable<string> GetTextValues() {
        yield return Id;
        yield return Name;
    }
    
    protected override IEnumerable<object> GetAtomicValues() {
        yield return Id;
    }

    protected string ToUniqueId(string id, Guid contentKey) {
        var idSuffixes = Suffixes.GetOrAdd(id, () => new ConcurrentDictionary<Guid, int>());
        var suffix = idSuffixes.GetOrAdd(contentKey, () => idSuffixes.Count);
        
        if (suffix != 0) {
            id += suffix;
        }
        
        return id;
    }
    
    private string GetId() {
        var content = Content();
        var id = content.Name;

        if (id == id.ToUpperInvariant()) {
            id = id.ToLowerInvariant();
        }

        id = Regex.Replace(id, "[^0-9a-z-_]", "", RegexOptions.CultureInvariant|RegexOptions.IgnoreCase).Camelize();

        return ToUniqueId(id, content.Key);
    }
}
