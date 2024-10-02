using N3O.Umbraco.Financial;
using System.Collections.Generic;

namespace N3O.Umbraco.Crowdfunding.Models;

public class ContributionStatisticsRes {
    public MoneyRes Total { get; set; }
    public MoneyRes Average { get; set; }
    public int Count { get; set; }
    
    public IEnumerable<DailyContributionStatisticsRes> Daily { get; set; }
}