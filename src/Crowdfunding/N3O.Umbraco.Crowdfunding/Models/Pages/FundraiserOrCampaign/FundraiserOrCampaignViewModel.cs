﻿using N3O.Umbraco.Crowdfunding;
using N3O.Umbraco.CrowdFunding.Extensions;
using N3O.Umbraco.Crowdfunding.UIBuilder;
using System.Collections.Generic;

namespace N3O.Umbraco.CrowdFunding.Models;

public abstract partial class FundraiserOrCampaignViewModel<TContent> : CrowdfundingViewModel<TContent> {
    public IReadOnlyList<Allocation> Allocations { get; set; }
    public ProgressInfo Progress { get; set; }

    public IEnumerable<Contribution> Contributions { get; set; }

    public static T For<T>(ICrowdfundingHelper crowdfundingHelper,
                           TContent content,
                           IEnumerable<CrowdfundingContribution> contributions)
        where T : FundraiserOrCampaignViewModel<TContent>, new() {
        var viewModel = new T();
        viewModel.Content = content;
        viewModel.Contributions = contributions.ToReadOnlyList(x => Contribution.For(crowdfundingHelper, x));
        
        return viewModel;
    }
}