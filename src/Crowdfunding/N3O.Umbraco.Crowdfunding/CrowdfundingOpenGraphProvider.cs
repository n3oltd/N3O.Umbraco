using N3O.Umbraco.Extensions;
using N3O.Umbraco.OpenGraph;
using System;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Crowdfunding;

public class CrowdfundingOpenGraphProvider : IOpenGraphProvider {
    private readonly Lazy<ICrowdfundingRouter> _crowdfundingRouter;

    public CrowdfundingOpenGraphProvider(Lazy<ICrowdfundingRouter> crowdfundingRouter) {
        _crowdfundingRouter = crowdfundingRouter;
    }
    
    public bool IsProviderFor(IPublishedContent page) {
        return page.ContentType.Alias.EqualsInvariant(CrowdfundingConstants.HomePage.Alias);
    }
    
    public void AddOpenGraph(IOpenGraphBuilder builder, IPublishedContent page) {
        _crowdfundingRouter.Value.CurrentPage.AddOpenGraph(builder,
                                                           _crowdfundingRouter.Value.RequestUri,
                                                           _crowdfundingRouter.Value.RequestQuery);
    }
}