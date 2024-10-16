using N3O.Umbraco.Attributes;
using N3O.Umbraco.Crowdfunding.Criteria;
using N3O.Umbraco.Crowdfunding.Entities;
using N3O.Umbraco.Crowdfunding.Models;
using System.Threading.Tasks;
using Umbraco.Cms.Infrastructure.Persistence;
using NPoco;
using System.Collections.Generic;

namespace N3O.Umbraco.Crowdfunding.Handlers;

public partial class GetDashboardStatisticsHandler {
    private async Task PopulateAllocationsAsync(IUmbracoDatabase db, DashboardStatisticsCriteria criteria,
                                                DashboardStatisticsRes res) {
        var allocationRows =await GetAllocationRowAsync(db, criteria);
        
        var topAllocations = new List<AllocationStatisticsItemRes>();
        
        foreach (var row in allocationRows) {
            var allocationRes = new AllocationStatisticsItemRes();
            allocationRes.Summary = row.AllocationSummary;
            allocationRes.Total = GetMoneyRes(row.TotalIncome);
            
            topAllocations.Add(allocationRes);
        }
        
        res.Allocations = new AllocationStatisticsRes();
        res.Allocations.TopItems = topAllocations;
    }

    private async Task<IReadOnlyList<AllocationStatisticsRow>> GetAllocationRowAsync(IUmbracoDatabase db,
                                                                                     DashboardStatisticsCriteria criteria) {
        var from = criteria.Period?.From?.ToDateTimeUnspecified();
        var to = criteria.Period?.To?.ToDateTimeUnspecified();
        
        var sql = Sql.Builder
                     .Select($"TOP (5) {nameof(Contribution.AllocationSummary)}")
                     .Append($", SUM({nameof(Contribution.BaseAmount)} + {nameof(Contribution.TaxReliefBaseAmount)}) AS {nameof(AllocationStatisticsRow.TotalIncome)}")
                     .From($"{CrowdfundingConstants.Tables.Contributions.Name}")
                     .Where($"{nameof(Contribution.Date)} BETWEEN @0 AND @1", from, to)
                     .GroupBy($"{nameof(Contribution.AllocationSummary)}")
                     .OrderBy($"{nameof(AllocationStatisticsRow.TotalIncome)} DESC");
        
        var rows = await db.FetchAsync<AllocationStatisticsRow>(sql);

        return rows;
    }
    
    private class AllocationStatisticsRow {
        [Order(1)]
        public string AllocationSummary { get; set; }

        [Order(2)]
        public decimal TotalIncome { get; set; }
    }
}