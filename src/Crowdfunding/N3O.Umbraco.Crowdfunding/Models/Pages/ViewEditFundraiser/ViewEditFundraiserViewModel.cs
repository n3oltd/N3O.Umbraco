using N3O.Umbraco.Constants;
using N3O.Umbraco.Crowdfunding;
using N3O.Umbraco.Crowdfunding.Content;
using N3O.Umbraco.Crowdfunding.Entities;
using N3O.Umbraco.Localization;
using System.Collections.Generic;
using Umbraco.Extensions;

namespace N3O.Umbraco.CrowdFunding.Models;

public class ViewEditFundraiserViewModel : FundraiserOrCampaignViewModel<FundraiserContent> {
    public static ViewEditFundraiserViewModel For(ICrowdfundingHelper crowdfundingHelper,
                                                  IFormatter formatter,
                                                  FundraiserContent fundraiser,
                                                  IEnumerable<OnlineContribution> onlineContributions) {
        var viewModel = For<ViewEditFundraiserViewModel>(crowdfundingHelper,
                                                         formatter,
                                                         fundraiser,
                                                         onlineContributions,
                                                         () => GetOwnerInfo(fundraiser));
        
        return viewModel;
    }
    
    private static FundraiserOrCampaignOwnerViewModel GetOwnerInfo(FundraiserContent fundraiser) {
        var name = fundraiser.Owner?.Name;
        var avatarLink = fundraiser.Owner?.Value<string>(MemberConstants.Member.Properties.AvatarLink);

        return FundraiserOrCampaignOwnerViewModel.For(name, avatarLink);
    }
}