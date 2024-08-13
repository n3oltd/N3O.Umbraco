using N3O.Umbraco.Crowdfunding;
using N3O.Umbraco.Crowdfunding.Content;
using N3O.Umbraco.CrowdFunding.Extensions;
using N3O.Umbraco.Crowdfunding.UIBuilder;
using System.Collections.Generic;

namespace N3O.Umbraco.CrowdFunding.Models;

public partial class FundraiserViewModel : FundraiserOrCampaignViewModel<FundraiserContent> {
    public OwnerInfo Owner { get; set; }
    
    public static FundraiserViewModel For(ICrowdfundingHelper crowdfundingHelper,
                                          FundraiserContent fundraiser,
                                          IEnumerable<CrowdfundingContribution> contributions) {
        var viewModel = For<FundraiserViewModel>(crowdfundingHelper, fundraiser, contributions);
        
        // TODO Talha this is needed on CampaignViewModel also, so should be refactored to avoid the hard dependency
        // on fundraiser
        viewModel.Allocations = fundraiser.Allocations.ToReadOnlyList(x => Allocation.For(crowdfundingHelper, x));
        viewModel.Progress = ProgressInfo.For(crowdfundingHelper, contributions, fundraiser);
        viewModel.Owner = OwnerInfo.For(fundraiser);
        
        return viewModel;
    }
}