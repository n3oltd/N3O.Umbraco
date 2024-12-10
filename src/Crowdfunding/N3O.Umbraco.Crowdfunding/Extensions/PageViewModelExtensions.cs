using N3O.Umbraco.Crowdfunding.Modules;
using N3O.Umbraco.Pages;

namespace N3O.Umbraco.Crowdfunding.Extensions;

public static class PageViewModelExtensions {
    public static CrowdfundingModuleData Crowdfunding(this IPageViewModel pageViewModel) {
        return pageViewModel.ModulesData.Get<CrowdfundingModuleData>(CrowdfundingConstants.ModuleKeys.Page);
    }
}