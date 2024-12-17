using N3O.Umbraco.Extensions;
using N3O.Umbraco.PageTitle;
using System;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Crowdfunding;

public class CrowdfundingPageTitleProvider : IPageTitleProvider {
    private readonly Lazy<ICrowdfundingRouter> _crowdfundingRouter;

    public CrowdfundingPageTitleProvider(Lazy<ICrowdfundingRouter> crowdfundingRouter) {
        _crowdfundingRouter = crowdfundingRouter;
    }
    
    public bool IsProviderFor(IPublishedContent page) {
        return page.ContentType.Alias.EqualsInvariant(CrowdfundingConstants.HomePage.Alias);
    }
    
    public string GetPageTitle(IPublishedContent page) {
        return _crowdfundingRouter.Value.CurrentPage?.GetPageTitle(_crowdfundingRouter.Value.RequestUri,
                                                                   _crowdfundingRouter.Value.RequestQuery);
    }
}