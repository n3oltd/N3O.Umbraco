using N3O.Umbraco.Crowdfunding;
using N3O.Umbraco.Crowdfunding.Content;
using N3O.Umbraco.Crowdfunding.Entities;
using System;
using System.Collections.Generic;

namespace N3O.Umbraco.CrowdFunding.Models;

public class ViewCampaignViewModel : FundraiserOrCampaignViewModel<CampaignContent> {
    public static ViewCampaignViewModel For(ICrowdfundingHelper crowdfundingHelper,
                                            CampaignContent campaign,
                                            IEnumerable<OnlineContribution> onlineContributions) {
        var viewModel = For<ViewCampaignViewModel>(crowdfundingHelper, campaign, onlineContributions);
        viewModel.OwnerInfo = GetOwnerInfo(campaign);
        
        return viewModel;
    }
    
    private static FundraiserOrCampaignOwnerViewModel GetOwnerInfo(CampaignContent fundraiser) {
        //TODO Need to create charity profile info in crowdfunding settings
        throw new NotImplementedException();
    }

    public override FundraiserOrCampaignOwnerViewModel OwnerInfo { get; set; }
}