using N3O.Umbraco.Crowdfunding;
using N3O.Umbraco.Crowdfunding.Content;
using N3O.Umbraco.Crowdfunding.UIBuilder;
using System.Collections.Generic;

namespace N3O.Umbraco.CrowdFunding.Models;

public class CampaignViewModel : FundraiserOrCampaignViewModel<CampaignContent> {
    public static CampaignViewModel For(ICrowdfundingHelper crowdfundingHelper,
                                        CampaignContent campaign,
                                        IEnumerable<CrowdfundingContribution> contributions) {
        var viewModel = For<CampaignViewModel>(crowdfundingHelper,
                                               campaign,
                                               contributions);
        
        return viewModel;
    }
}