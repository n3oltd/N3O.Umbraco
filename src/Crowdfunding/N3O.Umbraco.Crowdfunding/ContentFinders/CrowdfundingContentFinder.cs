using N3O.Umbraco.Crowdfunding;
using N3O.Umbraco.Extensions;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Routing;

namespace N3O.Umbraco.CrowdFunding;

public class CrowdfundingContentFinder : IContentFinder {
    private readonly ICrowdfundingHelper _crowdfundingHelper;

    public CrowdfundingContentFinder(ICrowdfundingHelper crowdfundingHelper) {
        _crowdfundingHelper = crowdfundingHelper;
    }
    
    public Task<bool> TryFindContent(IPublishedRequestBuilder request) {
        var crowdfundingPath = _crowdfundingHelper.GetCrowdfundingPath(request.Uri);

        if (crowdfundingPath.HasValue()) {
            request.SetPublishedContent(_crowdfundingHelper.GetCrowdfundingHomePage());

            return Task.FromResult(true);
        } else {
            return Task.FromResult(false);
        }
    }
}