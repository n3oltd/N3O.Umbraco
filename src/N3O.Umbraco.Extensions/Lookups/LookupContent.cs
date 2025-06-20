using Humanizer;
using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Lookups;

public static class LookupContent {
    private static readonly ConcurrentDictionary<string, ConcurrentDictionary<Guid, int>> Suffixes = new(StringComparer.InvariantCultureIgnoreCase);
    
    public static string GetId(IPublishedContent content) {
        var id = content.Name;

        if (id == id.ToUpperInvariant()) {
            id = id.ToLowerInvariant();
        }

        id = Regex.Replace(id, "[^0-9a-z-_]", "", RegexOptions.CultureInvariant|RegexOptions.IgnoreCase).Camelize();

        return ToUniqueId(id, content.Key);
    }
    
    public static string GetName(IPublishedContent content) {
        return content.Name;
    }
    
    public static string ToUniqueId(string id, Guid contentKey) {
        var idSuffixes = Suffixes.GetOrAdd(id, () => new ConcurrentDictionary<Guid, int>());
        var suffix = idSuffixes.GetOrAdd(contentKey, () => idSuffixes.Count);
        
        if (suffix != 0) {
            id += suffix;
        }
        
        return id;
    }
}

public abstract class LookupContent<T> : UmbracoContent<T>, INamedLookup {
    public virtual string Id => LookupContent.GetId(Content());
    public virtual string Name => LookupContent.GetName(Content());

    public IEnumerable<string> GetTextValues() {
        yield return Id;
        yield return Name;
    }
    
    protected override IEnumerable<object> GetAtomicValues() {
        yield return Id;
    }
}
