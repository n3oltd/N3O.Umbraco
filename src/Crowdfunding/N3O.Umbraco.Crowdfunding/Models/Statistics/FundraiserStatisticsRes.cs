using N3O.Umbraco.Financial;

namespace N3O.Umbraco.Crowdfunding.Models;

public class FundraiserStatisticsRes {
    public MoneyRes ContributionsTotal { get; set; }
    public FundraiserStatisticsItemRes TopItems { get; set; }
    public int ActiveCount { get; set; }
    public MoneyRes GoalsTotal { get; set; }
}