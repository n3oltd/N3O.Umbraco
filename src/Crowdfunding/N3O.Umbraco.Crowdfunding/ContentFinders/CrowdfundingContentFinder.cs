using N3O.Umbraco.Content;
using N3O.Umbraco.Crowdfunding;
using N3O.Umbraco.Extensions;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Routing;

namespace N3O.Umbraco.CrowdFunding;

public class CrowdfundingContentFinder : IContentFinder {
    private readonly IContentLocator _contentLocator;

    public CrowdfundingContentFinder(IContentLocator contentLocator) {
        _contentLocator = contentLocator;
    }
    
    public Task<bool> TryFindContent(IPublishedRequestBuilder request) {
        var crowdfundingPath = CrowdfundingHelper.GetCrowdfundingPath(_contentLocator, request.Uri);

        if (crowdfundingPath.HasValue()) {
            request.SetPublishedContent(CrowdfundingHelper.GetCrowdfundingHomePage(_contentLocator));

            return Task.FromResult(true);
        } else {
            return Task.FromResult(false);
        }
    }
}