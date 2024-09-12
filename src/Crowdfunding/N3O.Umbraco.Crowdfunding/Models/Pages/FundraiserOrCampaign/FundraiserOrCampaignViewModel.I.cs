using System.Collections.Generic;

namespace N3O.Umbraco.CrowdFunding.Models;

public interface IFundraiserOrCampaignViewModel : ICrowdfundingViewModel {
    public IReadOnlyList<FundraiserOrCampaignGoalViewModel> Goals { get; }
    public FundraiserOrCampaignProgressViewModel FundraiserOrCampaignProgress { get; }
    public IReadOnlyList<FundraiserOrCampaignContributionViewModel> Contributions { get; }
    public FundraiserOrCampaignOwnerViewModel OwnerInfo { get; }
    public IReadOnlyList<string> Tags { get; }
}