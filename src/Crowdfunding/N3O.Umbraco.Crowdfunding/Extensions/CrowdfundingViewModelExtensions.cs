using N3O.Umbraco.Crowdfunding.Content;
using N3O.Umbraco.Crowdfunding.Models;

namespace N3O.Umbraco.Crowdfunding.Extensions;

public static class CrowdfundingViewModelExtensions {
    public static CampaignContent CurrentCampaign(this ICrowdfundingViewModel crowdfundingViewModel) {
        if (crowdfundingViewModel is ViewCampaignViewModel viewCampaignViewModel) {
            return viewCampaignViewModel.Content;
        } else {
            return null;
        }
    }
}