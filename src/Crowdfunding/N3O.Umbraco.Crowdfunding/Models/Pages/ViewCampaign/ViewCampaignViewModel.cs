using N3O.Umbraco.Crowdfunding;
using N3O.Umbraco.Crowdfunding.Content;
using N3O.Umbraco.Crowdfunding.Entities;
using N3O.Umbraco.Localization;
using System;
using System.Collections.Generic;

namespace N3O.Umbraco.CrowdFunding.Models;

public class ViewCampaignViewModel : FundraiserOrCampaignViewModel<CampaignContent> {
    public static ViewCampaignViewModel For(ICrowdfundingHelper crowdfundingHelper,
                                            IFormatter formatter,
                                            CampaignContent campaign,
                                            IEnumerable<OnlineContribution> onlineContributions) {
        var viewModel = For<ViewCampaignViewModel>(crowdfundingHelper,
                                                   formatter,
                                                   campaign,
                                                   onlineContributions,
                                                   () => GetOwnerInfo(campaign));
        
        return viewModel;
    }
    
    private static FundraiserOrCampaignOwnerViewModel GetOwnerInfo(CampaignContent campaign) {
        //TODO Need to create charity profile info in crowdfunding settings
        throw new NotImplementedException();
    }
}