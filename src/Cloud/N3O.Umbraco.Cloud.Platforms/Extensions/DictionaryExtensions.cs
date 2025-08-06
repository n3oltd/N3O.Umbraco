using N3O.Umbraco.Cloud.Platforms.Clients;
using System.Collections.Generic;

namespace N3O.Umbraco.Cloud.Platforms.Extensions;

public static class DictionaryExtensions {
    public static PublishedTagCollection ToPublishedTagCollection(this IReadOnlyDictionary<string, string> tags) {
        if (tags == null) {
            return null;
        }
        
        var res = new PublishedTagCollection();
        res.Entries = tags;

        return res;
    }
}