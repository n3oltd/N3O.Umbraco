using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using System;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Extensions;

namespace N3O.Umbraco.Crowdfunding;

public partial class CrowdfundingHelper {
    private static IPublishedContent _crowdfundingHomePage;
    private static string _crowdfundingHomePath;
    
    public static IPublishedContent GetCrowdfundingHomePage(IContentLocator contentLocator) {
        if (_crowdfundingHomePage == null) {
            _crowdfundingHomePage = contentLocator.Single(CrowdfundingConstants.CrowdfundingHomePage.Alias);
        }

        return _crowdfundingHomePage;
    }
    
    public static string GetCrowdfundingPath(IContentLocator contentLocator, Uri requestUri) {
        var crowdfundingHomePath = GetCrowdfundingHomePath(contentLocator);
        var requestedPath = requestUri.GetAbsolutePathDecoded().ToLowerInvariant();
        
        if (crowdfundingHomePath.HasValue() && requestedPath.StartsWith(crowdfundingHomePath)) {
            return requestedPath.Substring(crowdfundingHomePath.Length);
        } else {
            return null;
        }
    }
    
    private static string GetCrowdfundingHomePath(IContentLocator contentLocator) {
        if (_crowdfundingHomePath == null) {
            var crowdfundingHomePage = GetCrowdfundingHomePage(contentLocator);
            
            if (crowdfundingHomePage.HasValue()) {
                _crowdfundingHomePath = crowdfundingHomePage.RelativeUrl().TrimEnd("/");
            }
        }

        return _crowdfundingHomePath;
    }
}