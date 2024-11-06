using System.Collections.Generic;

namespace N3O.Umbraco.Crowdfunding.Models;

public class FundraiserStatisticsRes : CrowdfunderStatisticsRes {
    public int ActiveCount { get; set; }
    public int NewCount { get; set; }
    public int CompletedCount { get; set; }
    public IEnumerable<FundraiserByCampaignStatisticsRes> ByCampaign { get; set; }
}