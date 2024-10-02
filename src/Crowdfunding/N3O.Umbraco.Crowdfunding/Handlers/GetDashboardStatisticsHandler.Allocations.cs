using N3O.Umbraco.Crowdfunding.Criteria;
using N3O.Umbraco.Crowdfunding.Models;
using System.Threading.Tasks;
using Umbraco.Cms.Infrastructure.Persistence;

namespace N3O.Umbraco.Crowdfunding.Handlers;

public partial class GetDashboardStatisticsHandler {
    private async Task PopulateAllocationsAsync(IUmbracoDatabase db,
                                                DashboardStatisticsCriteria criteria,
                                                DashboardStatisticsRes res) {
        /*
SELECT TOP 5
    AllocationSummary,
    SUM(BaseAmount + TaxReliefBaseAmount) AS TotalIncome,
    COUNT(*) AS DonationCount
FROM Contributions
WHERE Date BETWEEN @StartDate AND @EndDate
GROUP BY AllocationSummary
ORDER BY TotalIncome DESC;
         */
        
        // Alias columns, group by date and use SQL parameters in SDK for the start and end dates
        var topItems = await db.FetchAsync<AllocationStatisticsItemRes>($"SELECT * FROM");

        res.Allocations = new AllocationStatisticsRes();
        res.Allocations.TopItems = topItems;
    }
}