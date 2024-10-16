using N3O.Umbraco.Attributes;
using N3O.Umbraco.Crowdfunding.Criteria;
using N3O.Umbraco.Crowdfunding.Entities;
using N3O.Umbraco.Crowdfunding.Models;
using N3O.Umbraco.Extensions;
using System.Linq;
using System.Threading.Tasks;
using NPoco;
using System;
using System.Collections.Generic;
using Umbraco.Cms.Infrastructure.Persistence;

namespace N3O.Umbraco.Crowdfunding.Handlers;

public partial class GetDashboardStatisticsHandler {
    private async Task PopulateContributionsAsync(IUmbracoDatabase db,
                                                  DashboardStatisticsCriteria criteria,
                                                  DashboardStatisticsRes res) {
        var contributionRows =await GetContributionRowAsync(db, criteria);
        
        var dailyContributions = new List<DailyContributionStatisticsRes>();
        
        foreach (var row in contributionRows) {
            var allocationRes = new DailyContributionStatisticsRes();
            allocationRes.Date = row.Date.ToLocalDate();
            allocationRes.Count = row.Count;
            allocationRes.Total = GetMoneyRes(row.TotalAmount);
            
            dailyContributions.Add(allocationRes);
        }
        
        var totalAmount = dailyContributions.Sum(x => x.Total.Amount);
        var totalCount = dailyContributions.Sum(x => x.Count);
        var averageAmount = totalCount > 0 ? totalAmount / totalCount : 0; 

        res.Contributions = new ContributionStatisticsRes();
        res.Contributions.Count = totalCount;
        res.Contributions.Total = GetMoneyRes(totalAmount);
        res.Contributions.Average = GetMoneyRes(averageAmount);
        res.Contributions.Daily = dailyContributions;
    }
    
    private async Task<IReadOnlyList<ContributionStatisticsRow>> GetContributionRowAsync(IUmbracoDatabase db,
                                                                                         DashboardStatisticsCriteria criteria) {
        var sqlQuery = Sql.Builder
                          .Select($"{nameof(Contribution.Date)}")
                          .Append($", SUM({nameof(Contribution.BaseAmount)} + {nameof(Contribution.TaxReliefBaseAmount)}) AS {nameof(ContributionStatisticsRow.TotalAmount)}")
                          .Append($", COUNT(*) AS {nameof(ContributionStatisticsRow.Count)}")
                          .From($"{CrowdfundingConstants.Tables.Contributions.Name}")
                          .Where($"{nameof(Contribution.Date)} BETWEEN @0 AND @1", criteria.Period.From.Value.ToDateTimeUnspecified(), criteria.Period.To.Value.ToDateTimeUnspecified())
                          .GroupBy($"{nameof(Contribution.Date)}")
                          .OrderBy($"{nameof(Contribution.Date)}");
        
        var rows = await db.FetchAsync<ContributionStatisticsRow>(sqlQuery);

        return rows;
    }
    
    private class ContributionStatisticsRow {
        [Order(1)]
        public DateTime Date { get; set; }

        [Order(2)]
        public decimal TotalAmount { get; set; }
        
        [Order(3)]
        public int Count { get; set; }
    }
}