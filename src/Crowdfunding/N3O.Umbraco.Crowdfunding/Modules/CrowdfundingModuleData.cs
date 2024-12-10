using N3O.Umbraco.Crowdfunding.Models;

namespace N3O.Umbraco.Crowdfunding.Modules;

public class CrowdfundingModuleData {
    public CrowdfundingModuleData(ICrowdfundingPage currentPage, ICrowdfundingViewModel viewModel) {
        CurrentPage = currentPage;
        ViewModel = viewModel;
    }

    public ICrowdfundingPage CurrentPage { get; }
    public ICrowdfundingViewModel ViewModel { get; }

    public static readonly CrowdfundingModuleData Empty = new(null, null);
}