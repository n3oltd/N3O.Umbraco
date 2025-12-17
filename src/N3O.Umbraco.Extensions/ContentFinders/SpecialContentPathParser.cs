using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using System;
using System.Collections.Generic;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Extensions;

namespace N3O.Umbraco.ContentFinders;

public static class SpecialContentPathParser {
    private static readonly Dictionary<SpecialContent, IPublishedContent> SpecialPages = new();
    private static readonly Dictionary<SpecialContent, string> SpecialPaths = new();
    
    public static string GetPath(IContentCache contentCache, SpecialContent specialContent) {
        var specialPath = SpecialPaths.GetOrAdd(specialContent, () => {
            var specialPage = GetPage(contentCache, specialContent);
            
            if (specialPage.HasValue()) {
                return specialPage.RelativeUrl().StripTrailingSlash();
            } else {
                return null;
            }  
        });

        return specialPath;
    }
    
    public static string ParseUri(IContentCache contentCache, SpecialContent specialContent, Uri requestUri) {
        var specialPath = GetPath(contentCache, specialContent);
        var requestedPath = requestUri.GetAbsolutePathDecoded().ToLowerInvariant().StripTrailingSlash();
        
        if (specialPath.HasValue() && requestedPath.StartsWith(specialPath)) {
            return requestedPath.Substring(specialPath.Length).EnsureTrailingSlash();
        } else {
            return null;
        }
    }
    
    private static IPublishedContent GetPage(IContentCache contentCache, SpecialContent specialContent) {
        var specialPage = SpecialPages.GetOrAdd(specialContent, () => contentCache.Special(specialContent));

        return specialPage;
    }
}