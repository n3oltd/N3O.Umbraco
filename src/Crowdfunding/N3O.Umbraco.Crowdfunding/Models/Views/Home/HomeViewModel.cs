using System.Threading.Tasks;

namespace N3O.Umbraco.Crowdfunding.Models;

public class HomeViewModel : CrowdfundingViewModel {
    public static async Task<HomeViewModel> ForAsync(ICrowdfundingViewModelFactory viewModelFactory,
                                                     HomePage page) {
        var viewModel = await viewModelFactory.CreateViewModelAsync<HomeViewModel>(page);

        return viewModel;
    }
}