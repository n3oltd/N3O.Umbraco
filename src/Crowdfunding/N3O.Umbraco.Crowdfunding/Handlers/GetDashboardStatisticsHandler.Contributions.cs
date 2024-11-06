using N3O.Umbraco.Attributes;
using N3O.Umbraco.Crowdfunding.Criteria;
using N3O.Umbraco.Crowdfunding.Entities;
using N3O.Umbraco.Crowdfunding.Models;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Giving.Lookups;
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
        var from = criteria.Period?.From?.ToDateTimeUnspecified();
        var to = criteria.Period?.To?.ToDateTimeUnspecified();
        
        var contributionRows = await GetContributionRowsAsync(db, from, to);
        
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

        var donationsCount = await GetDonationsCountAsync(db, from, to);

        res.Contributions = new ContributionStatisticsRes();
        res.Contributions.Count = totalCount;
        res.Contributions.Total = GetMoneyRes(totalAmount);
        res.Contributions.Average = GetMoneyRes(averageAmount);
        res.Contributions.Daily = dailyContributions;
        res.Contributions.SupportersCount = donationsCount.TotalDonationsCount;
        res.Contributions.SingleDonationsCount = donationsCount.SingleDonationsCount;
        res.Contributions.RegularDonationsCount = donationsCount.RegularDonationsCount;
    }
    
    private async Task<IReadOnlyList<ContributionStatisticsRow>> GetContributionRowsAsync(IUmbracoDatabase db,
                                                                                          DateTime? from,
                                                                                          DateTime? to) {
        var sqlQuery = Sql.Builder
                          .Select($"{nameof(Contribution.Date)} AS {nameof(ContributionStatisticsRow.Date)}")
                          .Append($", SUM({nameof(Contribution.BaseAmount)} + {nameof(Contribution.TaxReliefBaseAmount)}) AS {nameof(ContributionStatisticsRow.TotalAmount)}")
                          .Append($", COUNT(*) AS {nameof(ContributionStatisticsRow.Count)}")
                          .From($"{CrowdfundingConstants.Tables.Contributions.Name}")
                          .Where($"{nameof(Contribution.Date)} BETWEEN '{from}' AND '{to}'")
                          .GroupBy($"{nameof(Contribution.Date)}")
                          .OrderBy($"{nameof(Contribution.Date)}");
        
        var rows = await db.FetchAsync<ContributionStatisticsRow>(sqlQuery);

        return rows;
    }
    
    private async Task<DonationsStatistics> GetDonationsCountAsync(IUmbracoDatabase db,
                                                                   DateTime? from,
                                                                   DateTime? to) {
        var sqlQuery = Sql.Builder
                          .Select($"COUNT(DISTINCT CASE WHEN {nameof(Contribution.GivingTypeId)} = '{GivingTypes.Donation.Id}' THEN {nameof(Contribution.Email)} END) AS {nameof(DonationsStatistics.SingleDonationsCount)}")
                          .Append($", COUNT(DISTINCT CASE WHEN {nameof(Contribution.GivingTypeId)} = '{GivingTypes.RegularGiving.Id}' THEN {nameof(Contribution.Email)} END) AS {nameof(DonationsStatistics.RegularDonationsCount)}")
                          .Append($", COUNT(DISTINCT {nameof(Contribution.Email)}) AS {nameof(DonationsStatistics.TotalDonationsCount)}")
                          .From($"{CrowdfundingConstants.Tables.Contributions.Name}")
                          .Where($"{nameof(Contribution.Date)} BETWEEN '{from}' AND '{to}'");
        
        var donationsStatistics = await db.FetchAsync<DonationsStatistics>(sqlQuery);

        return donationsStatistics.Single();
    }
    
    private class ContributionStatisticsRow {
        [Order(1)]
        public DateTime Date { get; set; }

        [Order(2)]
        public decimal TotalAmount { get; set; }
        
        [Order(3)]
        public int Count { get; set; }
    }
    
    private class DonationsStatistics {
        [Order(1)]
        public int SingleDonationsCount { get; set; }

        [Order(2)]
        public int RegularDonationsCount { get; set; }
        
        [Order(3)]
        public int TotalDonationsCount { get; set; }
    }
}