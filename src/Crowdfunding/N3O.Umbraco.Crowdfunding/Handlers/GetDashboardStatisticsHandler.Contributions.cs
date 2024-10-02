using N3O.Umbraco.Crowdfunding.Criteria;
using N3O.Umbraco.Crowdfunding.Models;
using System.Linq;
using System.Threading.Tasks;
using Umbraco.Cms.Infrastructure.Persistence;

namespace N3O.Umbraco.Crowdfunding.Handlers;

public partial class GetDashboardStatisticsHandler {
    private async Task PopulateContributionsAsync(IUmbracoDatabase db,
                                                  DashboardStatisticsCriteria criteria,
                                                  DashboardStatisticsRes res) {
        /*
         * SELECT 
    SUM(BaseAmount + TaxReliefBaseAmount) AS TotalDonated,
    COUNT(*) AS DonationCount,
    AVG(BaseAmount + TaxReliefBaseAmount) AS AverageDonation
FROM Contributions
WHERE Date BETWEEN @StartDate AND @EndDate;
         */
        
        // Alias columns, group by date and use SQL parameters in SDK for the start and end dates
        var dailyContributions = await db.FetchAsync<DailyContributionStatisticsRes>($"SELECT * FROM");

        res.Contributions = new ContributionStatisticsRes();
        res.Contributions.Daily = dailyContributions.OrderBy(x => x.Date).ToList();
        //res.Total = res.Daily.Sum(x => x.Total).ToMoneyRes();
        res.Contributions.Count = dailyContributions.Sum(x => x.Count);
        // Convert to money, divide, then back to MoneyRes
        //res.Average = res.Total / res.Count;
    }
}