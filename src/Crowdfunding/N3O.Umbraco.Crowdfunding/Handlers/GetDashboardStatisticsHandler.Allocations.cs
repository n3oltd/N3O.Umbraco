using N3O.Umbraco.Attributes;
using N3O.Umbraco.Crowdfunding.Criteria;
using N3O.Umbraco.Crowdfunding.Entities;
using N3O.Umbraco.Crowdfunding.Extensions;
using N3O.Umbraco.Crowdfunding.Models;
using NPoco;
using System.Collections.Generic;
using System.Threading.Tasks;
using Umbraco.Cms.Infrastructure.Persistence;

namespace N3O.Umbraco.Crowdfunding.Handlers;

public partial class GetDashboardStatisticsHandler {
    private async Task PopulateAllocationsAsync(IUmbracoDatabase db,
                                                DashboardStatisticsCriteria criteria,
                                                DashboardStatisticsRes res) {
        var rows = await GetAllocationRowsAsync(db, criteria);
        
        var topAllocations = new List<AllocationStatisticsItemRes>();
        
        foreach (var row in rows) {
            var item = new AllocationStatisticsItemRes();
            item.Summary = row.AllocationSummary;
            item.Total = GetMoneyRes(row.TotalBaseIncome);
            
            topAllocations.Add(item);
        }
        
        res.Allocations = new AllocationStatisticsRes();
        res.Allocations.TopItems = topAllocations;
    }

    private async Task<IReadOnlyList<AllocationStatisticsRow>> GetAllocationRowsAsync(IUmbracoDatabase db,
                                                                                      DashboardStatisticsCriteria criteria) {
        var sql = Sql.Builder
                     .Select($"TOP (5) {nameof(Contribution.AllocationSummary)} AS {nameof(AllocationStatisticsRow.AllocationSummary)}")
                     .Append($", SUM({nameof(Contribution.BaseAmount)} + {nameof(Contribution.TaxReliefBaseAmount)}) AS {nameof(AllocationStatisticsRow.TotalBaseIncome)}")
                     .From($"{CrowdfundingConstants.Tables.Contributions.Name}")
                     .Where(criteria.Period.FilterColumn(nameof(Contribution.Date)))
                     .GroupBy($"{nameof(Contribution.AllocationSummary)}")
                     .OrderBy($"{nameof(AllocationStatisticsRow.TotalBaseIncome)} DESC");
        
        LogQuery(sql);
        
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