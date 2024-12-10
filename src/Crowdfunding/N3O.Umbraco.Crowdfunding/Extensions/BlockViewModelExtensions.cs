using N3O.Umbraco.Blocks;
using N3O.Umbraco.Crowdfunding.Modules;

namespace N3O.Umbraco.Crowdfunding.Extensions;

public static class BlockViewModelExtensions {
    public static CrowdfundingModuleData Crowdfunding(this IBlockViewModel blockViewModel) {
        return blockViewModel.ModulesData.Get<CrowdfundingModuleData>(CrowdfundingConstants.ModuleKeys.Block);
    }
}
