using N3O.Umbraco.Crowdfunding.Criteria;
using N3O.Umbraco.Crowdfunding.Models;
using System.Linq;
using System.Threading.Tasks;
using Umbraco.Cms.Infrastructure.Persistence;
using N3O.Umbraco.Financial;
using NPoco;

namespace N3O.Umbraco.Crowdfunding.Handlers;

public partial class GetDashboardStatisticsHandler {
    private async Task PopulateAllocationsAsync(IUmbracoDatabase db,
        DashboardStatisticsCriteria criteria,
        DashboardStatisticsRes res) {

        var sql = Sql.Builder
            .Select("TOP 5 JSON_VALUE(AllocationJson, '$.allocationName') AS Summary, SUM(BaseAmount + TaxReliefBaseAmount) AS Total")
            .From("Contributions")
            .Where("Date BETWEEN @0 AND @1", criteria.Period.From, criteria.Period.To)
            .GroupBy("JSON_VALUE(AllocationJson, '$.allocationName')")
            .OrderBy("Total DESC");
        
        var rawResults = await db.FetchAsync<dynamic>(sql);
        
        var topItems = rawResults.Select(result => new AllocationStatisticsItemRes {
            Summary = result.Summary,
            Total = new MoneyRes {
                Amount = result.Total,
                Text = $"{result.Total}" 
            }
        }).ToList();
        
        res.Allocations = new AllocationStatisticsRes {
            TopItems = topItems
        };
    }
}