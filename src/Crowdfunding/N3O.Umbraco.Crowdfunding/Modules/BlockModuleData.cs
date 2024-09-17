using N3O.Umbraco.Crowdfunding.Models;

namespace N3O.Umbraco.Crowdfunding.Modules;

public class BlockModuleData {
    public BlockModuleData(ICrowdfundingPage currentPage, ICrowdfundingViewModel viewModel) {
        CurrentPage = currentPage;
        ViewModel = viewModel;
    }

    public ICrowdfundingPage CurrentPage { get; }
    public ICrowdfundingViewModel ViewModel { get; }

    public static readonly BlockModuleData Empty = new(null, null);
}