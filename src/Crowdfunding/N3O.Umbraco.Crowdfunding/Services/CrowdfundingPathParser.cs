using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using System;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Extensions;

namespace N3O.Umbraco.Crowdfunding;

public static class CrowdfundingPathParser {
    private static IPublishedContent _homePage;
    private static string _homePath;
    
    public static string ParseUri(IContentLocator contentLocator, Uri requestUri) {
        var homePath = GetHomePath(contentLocator);
        var requestedPath = requestUri.GetAbsolutePathDecoded().ToLowerInvariant().StripTrailingSlash();
        
        if (homePath.HasValue() && requestedPath.StartsWith(homePath)) {
            return requestedPath.Substring(homePath.Length).EnsureTrailingSlash();
        } else {
            return null;
        }
    }
    
    private static string GetHomePath(IContentLocator contentLocator) {
        if (_homePath == null) {
            var homePage = GetHomePage(contentLocator);
            
            if (homePage.HasValue()) {
                _homePath = homePage.RelativeUrl().StripTrailingSlash();
            }
        }

        return _homePath;
    }
    
    private static IPublishedContent GetHomePage(IContentLocator contentLocator) {
        if (_homePage == null) {
            _homePage = contentLocator.Single(CrowdfundingConstants.HomePage.Alias);
        }

        return _homePage;
    }
}