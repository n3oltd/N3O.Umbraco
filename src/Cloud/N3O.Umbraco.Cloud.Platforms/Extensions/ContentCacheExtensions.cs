using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Lookups;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Cloud.Platforms.Extensions;

public static class ContentCacheExtensions {
    private static IPublishedContent _donatePage;
    private static string _donatePath;
    
    public static string GetDonatePath(this IContentCache contentCache) {
        if (_donatePath == null) {
            var donatePage = GetDonatePage(contentCache);
            
            if (donatePage.HasValue()) {
                _donatePath = donatePage.RelativeUrl().StripTrailingSlash();
            }
        }

        return _donatePath;
    }
    
    private static IPublishedContent GetDonatePage(IContentCache contentCache) {
        if (_donatePage == null) {
            _donatePage = contentCache.Special(SpecialPages.Donate);
        }

        return _donatePage;
    }
}