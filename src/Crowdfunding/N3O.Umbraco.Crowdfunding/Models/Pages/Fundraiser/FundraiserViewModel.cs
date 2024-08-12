using N3O.Umbraco.Crowdfunding;
using N3O.Umbraco.Crowdfunding.Content;
using N3O.Umbraco.CrowdFunding.Extensions;
using N3O.Umbraco.Crowdfunding.UIBuilder;
using System.Collections.Generic;

namespace N3O.Umbraco.CrowdFunding.Models.FundraisingPage;

public partial class FundraiserViewModel : CrowdfundingViewModel<FundraiserContent> {
    public IReadOnlyList<Allocation> Allocations { get; set; }
    public ProgressInfo Progress { get; set; }
    public OwnerInfo Owner { get; set; }
    public IEnumerable<Contribution> Contributions { get; set; }

    public static FundraiserViewModel For(ICrowdfundingHelper crowdfundingHelper,
                                          FundraiserContent fundraiser,
                                          IEnumerable<CrowdfundingContribution> contributions) {
        var viewModel = new FundraiserViewModel();
        
        viewModel.Content = fundraiser;
        viewModel.Allocations = fundraiser.Allocations.ToReadOnlyList(x => Allocation.For(crowdfundingHelper, x));
        viewModel.Progress = ProgressInfo.For(crowdfundingHelper, contributions, fundraiser);
        viewModel.Owner = OwnerInfo.For(fundraiser);
        viewModel.Contributions = contributions.ToReadOnlyList(x => Contribution.For(crowdfundingHelper, x));
        
        return viewModel;
    }
}