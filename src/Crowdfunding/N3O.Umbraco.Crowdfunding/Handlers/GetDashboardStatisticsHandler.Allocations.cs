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
        var allocationRows = await GetAllocationRowsAsync(db, criteria);
        
        var topAllocations = new List<AllocationStatisticsItemRes>();
        
        foreach (var row in allocationRows) {
            var allocationRes = new AllocationStatisticsItemRes();
            allocationRes.Summary = row.AllocationSummary;
            allocationRes.Total = GetMoneyRes(row.TotalBaseIncome);
            
            topAllocations.Add(allocationRes);
        }
        
        res.Allocations = new AllocationStatisticsRes();
        res.Allocations.TopItems = topAllocations;
    }

    private async Task<IReadOnlyList<AllocationStatisticsRow>> GetAllocationRowsAsync(IUmbracoDatabase db,
                                                                                      DashboardStatisticsCriteria criteria) {
        var from = criteria.Period?.From?.ToDateTimeUnspecified();
        var to = criteria.Period?.To?.ToDateTimeUnspecified();
        
        var sql = Sql.Builder
                     .Select($"TOP (5) {nameof(Contribution.AllocationSummary)} AS {nameof(AllocationStatisticsRow.AllocationSummary)}")
                     .Append($", SUM({nameof(Contribution.BaseAmount)} + {nameof(Contribution.TaxReliefBaseAmount)}) AS {nameof(AllocationStatisticsRow.TotalBaseIncome)}")
                     .From($"{CrowdfundingConstants.Tables.Contributions.Name}")
                     .Where($"{nameof(Contribution.Date)} BETWEEN '{from}' AND '{to}'")
                     .GroupBy($"{nameof(Contribution.AllocationSummary)}")
                     .OrderBy($"{nameof(AllocationStatisticsRow.TotalBaseIncome)} DESC");
        
        var rows = await db.FetchAsync<AllocationStatisticsRow>(sql);

        return rows;
    }
    
    private class AllocationStatisticsRow {
        [Order(1)]
        public string AllocationSummary { get; set; }

        [Order(2)]
        public decimal TotalBaseIncome { get; set; }
    }
}