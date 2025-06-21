using N3O.Umbraco.Cloud.Platforms.Clients;
using System.Collections.Generic;

namespace N3O.Umbraco.Cloud.Platforms.Extensions;

public static class DictionaryExtensions {
    public static PublishedAnalyticsParameters ToPublishedAnalyticsParameters(this IReadOnlyDictionary<string, string> tags) {
        if (tags == null) {
            return null;
        }
        
        var res = new PublishedAnalyticsParameters();
        res.Tags = tags;

        return res;
    }
}