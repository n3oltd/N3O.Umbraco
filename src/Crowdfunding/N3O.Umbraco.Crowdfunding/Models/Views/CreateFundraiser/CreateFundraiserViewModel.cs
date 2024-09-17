using N3O.Umbraco.Crowdfunding.Content;
using System.Threading.Tasks;

namespace N3O.Umbraco.Crowdfunding.Models;

public class CreateFundraiserViewModel : CrowdfundingViewModel {
    public CampaignContent Campaign { get; private set; }
    
    public static async Task<CreateFundraiserViewModel> ForAsync(ICrowdfundingViewModelFactory viewModelFactory,
                                                                 CreateFundraiserPage page,
                                                                 CampaignContent campaignContent) {
        var viewModel = await viewModelFactory.CreateViewModelAsync<CreateFundraiserViewModel>(page);
        viewModel.Campaign = campaignContent;

        return viewModel;
    }
}