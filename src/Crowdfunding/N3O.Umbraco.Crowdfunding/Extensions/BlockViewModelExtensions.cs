using N3O.Umbraco.Blocks;
using N3O.Umbraco.Crowdfunding.Modules;

namespace N3O.Umbraco.Crowdfunding.Extensions;

public static class BlockViewModelExtensions {
    public static BlockModuleData Crowdfunding(this IBlockViewModel blockViewModel) {
        return blockViewModel.ModulesData.Get<BlockModuleData>(CrowdfundingConstants.ModuleKeys.Block);
    }
}
