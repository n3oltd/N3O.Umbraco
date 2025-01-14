using N3O.Umbraco.Attributes;
using N3O.Umbraco.Crowdfunding.Criteria;
using N3O.Umbraco.Crowdfunding.Entities;
using N3O.Umbraco.Crowdfunding.Extensions;
using N3O.Umbraco.Crowdfunding.Models;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Giving.Allocations.Lookups;
using NPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Umbraco.Cms.Infrastructure.Persistence;

namespace N3O.Umbraco.Crowdfunding.Handlers;

public partial class GetDashboardStatisticsHandler {
    private async Task PopulateContributionsAsync(IUmbracoDatabase db,
                                                  DashboardStatisticsCriteria criteria,
                                                  DashboardStatisticsRes res) {
        var contributionRows = await GetContributionRowsAsync(db, criteria);
        
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

        var donationStatistics = await GetDonationStatisticsAsync(db, criteria);

        res.Contributions = new ContributionStatisticsRes();
        res.Contributions.Count = totalCount;
        res.Contributions.Total = GetMoneyRes(totalAmount);
        res.Contributions.Average = GetMoneyRes(averageAmount);
        res.Contributions.Daily = dailyContributions;
        res.Contributions.SupportersCount = donationStatistics.TotalDonationsCount;
        res.Contributions.SingleDonationsCount = donationStatistics.SingleDonationsCount;
        res.Contributions.RegularDonationsCount = donationStatistics.RegularDonationsCount;
    }
    
    private async Task<IReadOnlyList<ContributionStatisticsRow>> GetContributionRowsAsync(IUmbracoDatabase db,
                                                                                          DashboardStatisticsCriteria criteria) {
        var sqlQuery = Sql.Builder
                          .Select($"{nameof(Contribution.Date)} AS {nameof(ContributionStatisticsRow.Date)}")
                          .Append($", SUM({nameof(Contribution.BaseAmount)} + {nameof(Contribution.TaxReliefBaseAmount)}) AS {nameof(ContributionStatisticsRow.TotalAmount)}")
                          .Append($", COUNT(*) AS {nameof(ContributionStatisticsRow.Count)}")
                          .From($"{CrowdfundingConstants.Tables.Contributions.Name}")
                          .Where(criteria.Period.FilterColumn(nameof(Contribution.Date)))
                          .GroupBy($"{nameof(Contribution.Date)}")
                          .OrderBy($"{nameof(Contribution.Date)}");
        
        var rows = await db.FetchAsync<ContributionStatisticsRow>(sqlQuery);

        return rows;
    }
    
    private async Task<DonationStatisticsRow> GetDonationStatisticsAsync(IUmbracoDatabase db,
                                                                         DashboardStatisticsCriteria criteria) {
        var sqlQuery = Sql.Builder
                          .Select($"COUNT(DISTINCT CASE WHEN {nameof(Contribution.GivingTypeId)} = '{GivingTypes.Donation.Id}' THEN {nameof(Contribution.Email)} END) AS {nameof(DonationStatisticsRow.SingleDonationsCount)}")
                          .Append($", COUNT(DISTINCT CASE WHEN {nameof(Contribution.GivingTypeId)} = '{GivingTypes.RegularGiving.Id}' THEN {nameof(Contribution.Email)} END) AS {nameof(DonationStatisticsRow.RegularDonationsCount)}")
                          .Append($", COUNT(DISTINCT {nameof(Contribution.Email)}) AS {nameof(DonationStatisticsRow.TotalDonationsCount)}")
                          .From($"{CrowdfundingConstants.Tables.Contributions.Name}")
                          .Where(criteria.Period.FilterColumn(nameof(Contribution.Date)));
        
        LogQuery(sqlQuery);
        
        var donationsStatistics = await db.FetchAsync<DonationStatisticsRow>(sqlQuery);

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
    
    private class DonationStatisticsRow {
        [Order(1)]
        public int SingleDonationsCount { get; set; }

        [Order(2)]
        public int RegularDonationsCount { get; set; }
        
        [Order(3)]
        public int TotalDonationsCount { get; set; }
    }
}