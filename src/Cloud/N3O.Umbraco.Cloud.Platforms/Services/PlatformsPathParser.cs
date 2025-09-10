using N3O.Umbraco.Cloud.Platforms.Extensions;
using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using System;
using Umbraco.Extensions;

namespace N3O.Umbraco.Cloud.Platforms;

public static class PlatformsPathParser {
    public static string ParseUri(IContentCache contentCache, Uri requestUri) {
        var donatePath = contentCache.GetDonatePath();
        var requestedPath = requestUri.GetAbsolutePathDecoded().ToLowerInvariant().StripTrailingSlash();
        
        if (donatePath.HasValue() && requestedPath.StartsWith(donatePath)) {
            return requestedPath.Substring(donatePath.Length).EnsureTrailingSlash();
        } else {
            return null;
        }
    }
}