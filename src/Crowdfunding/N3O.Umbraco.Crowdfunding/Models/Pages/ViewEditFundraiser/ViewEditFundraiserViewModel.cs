using N3O.Umbraco.Constants;
using N3O.Umbraco.Crowdfunding;
using N3O.Umbraco.Crowdfunding.Content;
using N3O.Umbraco.Crowdfunding.Entities;
using N3O.Umbraco.CrowdFunding.Extensions;
using System.Collections.Generic;
using Umbraco.Extensions;

namespace N3O.Umbraco.CrowdFunding.Models;

public class ViewEditFundraiserViewModel : FundraiserOrCampaignViewModel<FundraiserContent> {
    public static ViewEditFundraiserViewModel For(ICrowdfundingHelper crowdfundingHelper,
                                                  FundraiserContent fundraiser,
                                                  IEnumerable<OnlineContribution> onlineContributions) {
        var viewModel = For<ViewEditFundraiserViewModel>(crowdfundingHelper, fundraiser, onlineContributions);
        viewModel.OwnerInfo = GetOwnerInfo(fundraiser);
        
        return viewModel;
    }
    
    private static FundraiserOrCampaignOwnerViewModel GetOwnerInfo(FundraiserContent fundraiser) {
        var owner = new FundraiserOrCampaignOwnerViewModel();
            
        owner.Name = fundraiser.Owner?.Name;
        owner.AvatarLink = fundraiser.Owner?.Value<string>(MemberConstants.Member.Properties.AvatarLink);

        return owner;
    }

    public override FundraiserOrCampaignOwnerViewModel OwnerInfo { get; set; }
}