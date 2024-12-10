using N3O.Umbraco.Pages;
using N3O.Umbraco.Extensions;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Crowdfunding.Modules;

public class PageModule : IPageModule {
    private readonly ICrowdfundingRouter _crowdfundingRouter;

    public PageModule(ICrowdfundingRouter crowdfundingRouter) {
        _crowdfundingRouter = crowdfundingRouter;
    }
    
    public bool ShouldExecute(IPublishedContent page) {
        return page.ContentType.Alias.EqualsInvariant(Key);
    } 

    public async Task<object> ExecuteAsync(IPublishedContent page, CancellationToken cancellationToken) {
        var viewModel = await _crowdfundingRouter.CurrentPage
                                                 .GetViewModelAsync(_crowdfundingRouter.RequestUri,
                                                                    _crowdfundingRouter.RequestQuery);

        return new CrowdfundingModuleData(_crowdfundingRouter.CurrentPage, viewModel);
    }

    public string Key => CrowdfundingConstants.ModuleKeys.Page;
}
