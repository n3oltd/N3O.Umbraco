using System.Collections.Generic;

namespace N3O.Umbraco.CrowdFunding.Models;

public interface IFundraiserOrCampaignViewModel : ICrowdfundingViewModel {
    public IReadOnlyList<FundraiserOrCampaignAllocationViewModel> Allocations { get; set; }
    public FundraiserOrCampaignProgressViewModel FundraiserOrCampaignProgress { get; set; }
    public IReadOnlyList<FundraiserOrCampaignContributionViewModel> Contributions { get; set; }
    public FundraiserOrCampaignOwnerViewModel OwnerInfo { get; set; }
    public IReadOnlyList<string> Tags { get; set; }
}