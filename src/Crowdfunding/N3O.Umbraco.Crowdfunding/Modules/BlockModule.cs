using N3O.Umbraco.Blocks;
using N3O.Umbraco.Extensions;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Crowdfunding.Modules;

public class BlockModule : IBlockModule {
    private readonly ICrowdfundingRouter _crowdfundingRouter;

    public BlockModule(ICrowdfundingRouter crowdfundingRouter) {
        _crowdfundingRouter = crowdfundingRouter;
    }
    
    public bool ShouldExecute(IPublishedElement block) {
        return block.ContentType.Alias.EqualsInvariant(CrowdfundingConstants.Block.Alias);
    } 

    public async Task<object> ExecuteAsync(IPublishedElement block, CancellationToken cancellationToken) {
        var viewModel = await _crowdfundingRouter.CurrentPage
                                                 .GetViewModelAsync(_crowdfundingRouter.RequestUri,
                                                                    _crowdfundingRouter.RequestQuery);

        return new BlockModuleData(_crowdfundingRouter.CurrentPage, viewModel);
    }

    public string Key => CrowdfundingConstants.ModuleKeys.Block;
}
