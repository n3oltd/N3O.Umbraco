using N3O.Umbraco.Content;
using N3O.Umbraco.Crm.Lookups;
using N3O.Umbraco.Crowdfunding.Content;
using N3O.Umbraco.Crowdfunding.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace N3O.Umbraco.Crowdfunding.Models;

public class ViewCampaignViewModel : CrowdfunderViewModel<CampaignContent> {
    public override bool EditMode() => false;

    public static async Task<ViewCampaignViewModel> ForAsync(ICrowdfundingViewModelFactory viewModelFactory,
                                                             IContentLocator contentLocator,
                                                             ViewCampaignPage page,
                                                             CampaignContent campaign,
                                                             IEnumerable<Contribution> contributions) {
        var viewModel = await ForAsync<ViewCampaignViewModel>(viewModelFactory,
                                                              page,
                                                              campaign,
                                                              CrowdfunderTypes.Campaign,
                                                              contributions,
                                                              () => GetOwnerInfo(contentLocator));
        
        return viewModel;
    }
    
    private static CrowdfunderOwnerViewModel GetOwnerInfo(IContentLocator contentLocator) {
        var ourProfile = contentLocator.Single<OurProfileSettingsContent>();

        return CrowdfunderOwnerViewModel.For(ourProfile.DisplayName, ourProfile.ProfileImage.Src, ourProfile.Strapline);
    }
}