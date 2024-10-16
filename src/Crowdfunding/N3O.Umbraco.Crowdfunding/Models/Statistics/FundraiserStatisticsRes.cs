using System.Collections.Generic;

namespace N3O.Umbraco.Crowdfunding.Models;

//TOP RIght Where counts
public class FundraiserStatisticsRes : CrowdfunderStatisticsRes {
    public int ActiveCount { get; set; }
    public int NewCount { get; set; }
    public int CompletedCount { get; set; }
    
    //Secon chart on bottom
    public IEnumerable<FundraiserByCampaignStatisticsRes> ByCampaign { get; set; }
}