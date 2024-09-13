using N3O.Umbraco.Crowdfunding;
using N3O.Umbraco.Crowdfunding.Content;
using N3O.Umbraco.Crowdfunding.Entities;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Localization;
using System;
using System.Collections.Generic;
using System.Linq;

namespace N3O.Umbraco.CrowdFunding.Models;

public abstract class FundraiserOrCampaignViewModel<TContent> :
    CrowdfundingViewModel<TContent>, IFundraiserOrCampaignViewModel
    where TContent : CrowdfunderContent<TContent> {
    public IReadOnlyList<FundraiserOrCampaignGoalViewModel> Goals { get; private set; }
    public IReadOnlyList<FundraiserOrCampaignContributionViewModel> Contributions { get; private set; }
    public IReadOnlyList<string> Tags { get; private set; }
    public FundraiserOrCampaignProgressViewModel FundraiserOrCampaignProgress { get; private set; }
    public FundraiserOrCampaignOwnerViewModel OwnerInfo { get; private set; }

    public static T For<T>(ICrowdfundingHelper crowdfundingHelper,
                           IFormatter formatter,
                           TContent content,
                           IEnumerable<OnlineContribution> onlineContributions,
                           Func<FundraiserOrCampaignOwnerViewModel> getOwnerInfo)
        where T : FundraiserOrCampaignViewModel<TContent>, new() {
        var viewModel = new T();
        viewModel.Content = content;
        viewModel.Goals = content.Goals.ToReadOnlyList(x => FundraiserOrCampaignGoalViewModel.For(crowdfundingHelper, x));
        viewModel.Contributions = onlineContributions.ToReadOnlyList(x => FundraiserOrCampaignContributionViewModel.For(crowdfundingHelper, formatter, x));
        viewModel.Tags = content.Goals.SelectMany(x => x.Tags.Select(x => x.Name)).ToList();
        viewModel.FundraiserOrCampaignProgress = FundraiserOrCampaignProgressViewModel.For(crowdfundingHelper, onlineContributions, content.Goals);
        viewModel.OwnerInfo = getOwnerInfo();
        
        return viewModel;
    }

    ICrowdfunderContent ICrowdfundingViewModel.Content => Content;
}