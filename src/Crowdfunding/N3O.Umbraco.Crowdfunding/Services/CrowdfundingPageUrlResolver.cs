using N3O.Umbraco.Content;
using N3O.Umbraco.Crowdfunding;
using N3O.Umbraco.Exceptions;
using N3O.Umbraco.Extensions;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Routing;
using Umbraco.Extensions;

namespace N3O.Umbraco.CrowdFunding.Services;

public class CrowdfundingPageUrlResolver : IContentFinder {
    private readonly IContentCache _contentCache;

    public CrowdfundingPageUrlResolver(IContentCache contentCache) {
        _contentCache = contentCache;
    }
    
    public Task<bool> TryFindContent(IPublishedRequestBuilder request) {
        var crowdfundingPage = _contentCache.Single(CrowdfundingConstants.FundraisingPage.Alias);

        if (!crowdfundingPage.HasValue()) {
            return Task.FromResult(false);
        }
        
        CrowdfundingPageUrlConstants.SetRootPath(crowdfundingPage.RelativeUrl().TrimEnd("/"));
        
        var crowdfundingPageUrl = crowdfundingPage.RelativeUrl();
        var requestedPath = request.Uri.GetAbsolutePathDecoded().ToLowerInvariant();

        if (requestedPath.StartsWith(crowdfundingPageUrl)) {
            request.SetPublishedContent(crowdfundingPage);

            return Task.FromResult(true);
        } else {
            return Task.FromResult(false);
        }
    }
}