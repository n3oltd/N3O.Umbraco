using N3O.Umbraco.Crowdfunding;
using N3O.Umbraco.Crowdfunding.Content;
using N3O.Umbraco.Crowdfunding.Entities;
using N3O.Umbraco.CrowdFunding.Extensions;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Cms.Core.Models;

namespace N3O.Umbraco.CrowdFunding.Models;

public abstract class FundraiserOrCampaignViewModel<TContent> 
    : CrowdfundingViewModel<TContent>, IFundraiserOrCampaignViewModel where TContent : CrowdfunderContent<TContent> {
    public IReadOnlyList<FundraiserOrCampaignAllocationViewModel> Allocations { get; set; }
    public IReadOnlyList<FundraiserOrCampaignContributionViewModel> Contributions { get; set; }
    public IReadOnlyList<string> Tags { get; set; }
    public FundraiserOrCampaignProgressViewModel FundraiserOrCampaignProgress { get; set; }

    public static T For<T>(ICrowdfundingHelper crowdfundingHelper,
                           TContent content,
                           IEnumerable<OnlineContribution> onlineContributions)
        where T : FundraiserOrCampaignViewModel<TContent>, new() {
        var viewModel = new T();
        viewModel.Content = content;
        viewModel.Allocations = content.Goals.ToReadOnlyList(x => FundraiserOrCampaignAllocationViewModel.For(crowdfundingHelper, x));
        viewModel.Contributions = onlineContributions.ToReadOnlyList(x => FundraiserOrCampaignContributionViewModel.For(crowdfundingHelper, x));
        viewModel.Tags = content.Goals.SelectMany(x => x.Tags.Select(x => x.Name)).ToList();
        viewModel.FundraiserOrCampaignProgress = FundraiserOrCampaignProgressViewModel.For(crowdfundingHelper, onlineContributions, content.Goals);
        
        return viewModel;
    }

    public abstract FundraiserOrCampaignOwnerViewModel OwnerInfo { get; set; }

    ICrowdfunderContent ICrowdfundingViewModel.Content => Content;
}