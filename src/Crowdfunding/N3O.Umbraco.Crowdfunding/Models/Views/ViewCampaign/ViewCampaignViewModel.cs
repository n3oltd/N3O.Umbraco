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
    public IReadOnlyList<ViewCampaignFundraisersViewModel> FundraisersContributions { get; private set; }

    public static async Task<ViewCampaignViewModel> ForAsync(ICrowdfundingViewModelFactory viewModelFactory,
                                                             IContentLocator contentLocator,
                                                             ILookups lookups,
                                                             ViewCampaignPage page,
                                                             CampaignContent campaign,
                                                             IEnumerable<Contribution> campaignContributions,
                                                             List<Contribution> fundraisersContributions,
                                                             List<FundraiserContent> fundraisersContent) {
        var viewModel = await ForAsync<ViewCampaignViewModel>(viewModelFactory,
                                                              lookups,
                                                              page,
                                                              campaign,
                                                              CrowdfunderTypes.Campaign,
                                                              campaignContributions,
                                                              () => GetOwnerInfo(contentLocator));

        PopuateFundraiserContributions(viewModel, fundraisersContributions, fundraisersContent);
        
        return viewModel;
    }

    private static void PopuateFundraiserContributions(ViewCampaignViewModel viewModel,
                                                       List<Contribution> fundraisersContributions,
                                                       List<FundraiserContent> fundraisersContent) {
        if (!fundraisersContributions.HasAny()) {
            return;
        }
        
        var fundraisersContributionViewModel = new List<ViewCampaignFundraisersViewModel>();
        
        foreach (var fundraisersContribution in fundraisersContributions.GroupBy(x => x.FundraiserId)) {
            var fundraiser = fundraisersContent.Single(x => x.FundraiserId == fundraisersContribution.Key);
            
            fundraisersContributionViewModel.Add(ViewCampaignFundraisersViewModel.For(fundraiser, fundraisersContribution));
        }

        viewModel.FundraisersContributions = fundraisersContributionViewModel;
    }
    
    private static CrowdfunderOwnerViewModel GetOwnerInfo(IContentLocator contentLocator) {
        var ourProfile = contentLocator.Single<OurProfileSettingsContent>();

        return CrowdfunderOwnerViewModel.For(ourProfile.DisplayName, ourProfile.ProfileImage.Src, ourProfile.Strapline);
    }
}