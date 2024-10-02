using N3O.Umbraco.Financial;
using NodaTime;

namespace N3O.Umbraco.Crowdfunding.Models;

public class DailyContributionStatisticsRes {
    public LocalDate Date { get; set; }
    public MoneyRes Total { get; set; }
    public int Count { get; set; }
}