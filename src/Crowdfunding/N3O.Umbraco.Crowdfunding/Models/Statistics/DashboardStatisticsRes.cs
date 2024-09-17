using N3O.Umbraco.Financial;

namespace N3O.Umbraco.Crowdfunding.Models;

public class DashboardStatisticsRes {
    public CurrencyRes BaseCurrency { get; set; }
    public ContributionStatisticsRes Contributions { get; set; }
    public AllocationStatisticsRes Allocations { get; set; }
    public CampaignStatisticsRes Campaigns { get; set; }
    public FundraiserStatisticsRes Fundraisers { get; set; }
}