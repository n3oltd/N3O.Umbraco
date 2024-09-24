using N3O.Umbraco.Content;
using N3O.Umbraco.Crowdfunding.Content;
using N3O.Umbraco.Crowdfunding.Entities;
using N3O.Umbraco.Crowdfunding.Lookups;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Lookups;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace N3O.Umbraco.Crowdfunding.Models;

public class ViewCampaignViewModel : CrowdfunderViewModel<CampaignContent> {
    public override bool EditMode() => false;
    public IReadOnlyList<ViewCampaignFundraiserViewModel> Fundraisers { get; private set; }

    public static async Task<ViewCampaignViewModel> ForAsync(ICrowdfundingViewModelFactory viewModelFactory,
                                                             IContentLocator contentLocator,
                                                             ILookups lookups,
                                                             ViewCampaignPage page,
                                                             IReadOnlyDictionary<string, string> query,
                                                             CampaignContent campaign,
                                                             IEnumerable<Contribution> campaignContributions,
                                                             IReadOnlyList<Contribution> fundraiserContributions,
                                                             IReadOnlyList<FundraiserContent> fundraisers) {
        var viewModel = await ForAsync<ViewCampaignViewModel>(viewModelFactory,
                                                              lookups,
                                                              page,
                                                              query,
                                                              campaign,
                                                              CrowdfunderTypes.Campaign,
                                                              campaignContributions,
                                                              () => GetOwnerInfo(contentLocator));
        
        viewModel.Fundraisers = fundraisers.OrEmpty()
                                           .Select(x => ViewCampaignFundraiserViewModel.For(x,
                                                                                            fundraiserContributions.Where(c => c.FundraiserId == x.Key)))
                                           .OrderBy(x => x.ContributionsTotal.Amount)
                                           .ToList();
        
        return viewModel;
    }
    
    private static CrowdfunderOwnerViewModel GetOwnerInfo(IContentLocator contentLocator) {
        var ourProfile = contentLocator.Single<OurProfileSettingsContent>();

        return CrowdfunderOwnerViewModel.For(ourProfile.DisplayName, ourProfile.ProfileImage.Src, ourProfile.Strapline);
    }
}