using N3O.Umbraco.Crowdfunding.Criteria;
using N3O.Umbraco.Crowdfunding.Models;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using N3O.Umbraco.Financial;
using NPoco;
using Umbraco.Cms.Infrastructure.Persistence;

namespace N3O.Umbraco.Crowdfunding.Handlers;

public partial class GetDashboardStatisticsHandler {
    private async Task PopulateContributionsAsync(IUmbracoDatabase db,
        DashboardStatisticsCriteria criteria,
        DashboardStatisticsRes res)
    {

        var sqlQuery = Sql.Builder
            .Select("Date, SUM(BaseAmount + TaxReliefBaseAmount) AS Total,COUNT(*) AS Count")
            .From("Contributions")
            .Where("Date BETWEEN @0 AND @1", criteria.Period.From, criteria.Period.To)
            .GroupBy("Date")
            .OrderBy("Date");
        
        // Alias columns, group by date and use SQL parameters in SDK for the start and end dates
        var rawResults = await db.FetchAsync<dynamic>(sqlQuery);

        //for daily contributions
        var dailyContributions = rawResults.Select(result => new DailyContributionStatisticsRes{
            Date = result.Date,
            Total = new MoneyRes()
            {
                Amount = result.Total,
                Text = $"{result.Total}" 
            },
            Count = result.Count,
        }).ToList();
        
        //for the overall stats:
        var totalAmount = dailyContributions.Sum(x => x.Total.Amount); 
        var totalCount = dailyContributions.Sum(x => x.Count);         
        var averageAmount = totalCount > 0 ? totalAmount / totalCount : 0; 

        //result object
        res.Contributions = new ContributionStatisticsRes {
            Total = new MoneyRes {
                Amount = totalAmount,
                Text = $"{totalAmount}" 
            },
            Average = new MoneyRes {
                Amount = averageAmount,
                Text = $"{averageAmount}" 
            },
            Count = totalCount,
            Daily = dailyContributions.OrderBy(x => x.Date).ToList() 
        };
    }
}



