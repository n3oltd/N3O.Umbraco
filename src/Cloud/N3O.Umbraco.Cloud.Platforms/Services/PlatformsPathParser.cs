using N3O.Umbraco.Cloud.Platforms.Extensions;
using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Lookups;
using System;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Extensions;

namespace N3O.Umbraco.Cloud.Platforms;

public static class PlatformsPathParser {
    private static IPublishedContent _donatePage;
    private static string _donatePath;
    
    public static string GetDonatePath(IContentCache contentCache) {
        if (_donatePath == null) {
            var donatePage = GetDonatePage(contentCache);
            
            if (donatePage.HasValue()) {
                _donatePath = donatePage.RelativeUrl().StripTrailingSlash();
            }
        }

        return _donatePath;
    }
    
    public static bool IsPlatformsDonatePage(IContentCache contentCache, Uri requestUri) {
        var requestedPath = requestUri.GetAbsolutePathDecoded().ToLowerInvariant().StripTrailingSlash();
        var donatePath = GetDonatePath(contentCache);

        return requestedPath.StartsWith(donatePath) && !donatePath.EqualsInvariant(requestedPath);
    }
    
    public static string ParseUri(IContentCache contentCache, Uri requestUri) {
        var donatePath = GetDonatePath(contentCache);
        var requestedPath = requestUri.GetAbsolutePathDecoded().ToLowerInvariant().StripTrailingSlash();
        
        if (donatePath.HasValue() && requestedPath.StartsWith(donatePath)) {
            return requestedPath.Substring(donatePath.Length).EnsureTrailingSlash();
        } else {
            return null;
        }
    }
    
    private static IPublishedContent GetDonatePage(IContentCache contentCache) {
        if (_donatePage == null) {
            _donatePage = contentCache.Special(SpecialPages.Donate);
        }

        return _donatePage;
    }
}