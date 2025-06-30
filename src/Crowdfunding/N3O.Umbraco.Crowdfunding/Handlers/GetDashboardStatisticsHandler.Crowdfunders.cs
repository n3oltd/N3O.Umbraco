using N3O.Umbraco.Attributes;
using N3O.Umbraco.Cloud.Engage.Lookups;
using N3O.Umbraco.Crowdfunding.Criteria;
using N3O.Umbraco.Crowdfunding.Entities;
using N3O.Umbraco.Crowdfunding.Extensions;
using N3O.Umbraco.Crowdfunding.Models;
using N3O.Umbraco.Extensions;
using NPoco;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Umbraco.Cms.Infrastructure.Persistence;

namespace N3O.Umbraco.Crowdfunding.Handlers;

public partial class GetDashboardStatisticsHandler {
    private async Task PopulateCrowdfunderAsync(IUmbracoDatabase db,
                                                CrowdfunderType type,
                                                DashboardStatisticsCriteria criteria,
                                                CrowdfunderStatisticsRes res) {
        var topCrowdfunders = await GetTopCrowdfunderRowsAsync(db, type, criteria);
        var activeCrowdfundersCount = await GetActiveCrowdfundersCountAsync(db, type, criteria);
        var completedPercentage = await GetCompletedPercentageAsync(db, type, criteria);
        
        res.Count = activeCrowdfundersCount;
        res.AveragePercentageComplete = completedPercentage;
        res.TopItems = topCrowdfunders.Select(x => new CrowdfunderStatisticsItemRes {
            Name = x.Name,
            GoalsTotal = GetMoneyRes(x.GoalsTotal),
            ContributionsTotal = GetMoneyRes(x.ContributionsTotal),
            Url = x.Url
        });
    }

    private async Task<int> GetActiveCrowdfundersCountAsync(IUmbracoDatabase db,
                                                            CrowdfunderType type,
                                                            DashboardStatisticsCriteria criteria) {
        var totalActiveCrowdfundersQuery = Sql.Builder
                                              .Select($"COUNT(DISTINCT {nameof(CrowdfunderRevision.ContentKey)})")
                                              .From($"{CrowdfundingConstants.Tables.CrowdfunderRevisions.Name}")
                                              .Where($"{nameof(CrowdfunderRevision.Type)} = {(int) type.Key}")
                                              .Append(criteria.Period.From.HasValue()
                                                          ? $"AND {nameof(CrowdfunderRevision.ActiveFrom)} <= '{criteria.Period.To.GetValueOrThrow().ToYearMonthDayString()}'"
                                                          : "")
                                              .Append(criteria.Period.To.HasValue()
                                                           ? $"AND ({nameof(CrowdfunderRevision.ActiveTo)} IS NULL OR {nameof(CrowdfunderRevision.ActiveTo)} >= '{criteria.Period.From.GetValueOrThrow().ToYearMonthDayString()}')"
                                                           : "");
        
        LogQuery(totalActiveCrowdfundersQuery);
        
        var count = await db.ExecuteScalarAsync<int>(totalActiveCrowdfundersQuery);
        
        return count;
    }

    private async Task<IReadOnlyList<CrowdfunderRow>> GetTopCrowdfunderRowsAsync(IUmbracoDatabase db,
                                                                                 CrowdfunderType type,
                                                                                 DashboardStatisticsCriteria criteria) {
        var innerQuery = Sql.Builder
                            .Select($"{nameof(CrowdfunderRevision.ContentKey)} AS CrowdfunderRevision_ContentKey",
                                    $"{nameof(CrowdfunderRevision.Name)} AS CrowdfunderRevision_Name",
                                    $"{nameof(CrowdfunderRevision.GoalsTotalBase)} AS CrowdfunderRevision_GoalsTotalBase",
                                    $"{nameof(CrowdfunderRevision.Url)} AS CrowdfunderRevision_Url",
                                    $"{nameof(CrowdfunderRevision.Type)} AS CrowdfunderRevision_Type",
                                    $"ROW_NUMBER() OVER (PARTITION BY {nameof(CrowdfunderRevision.Id)} ORDER BY {nameof(CrowdfunderRevision.ContentRevision)} DESC) AS RowNum")
                            .From($"{CrowdfundingConstants.Tables.CrowdfunderRevisions.Name}")
                            .Where($"{nameof(CrowdfunderRevision.Type)} = {(int) type.Key}");

        var sql = new Sql()
                  .Select("TOP 10 CrowdfunderRevision_Name AS [Name], CrowdfunderRevision_GoalsTotalBase AS [GoalsTotal], SUM(BaseAmount + TaxReliefBaseAmount) AS [ContributionsTotal], CrowdfunderRevision_Url AS [Url]")
                  .From($"{CrowdfundingConstants.Tables.Contributions.Name}")
                  .InnerJoin($"({innerQuery.SQL}) AS FG")
                  .On($"{nameof(Contribution.CrowdfunderId)} = CrowdfunderRevision_ContentKey")
                  .Where($"RowNum = 1 AND CrowdfunderRevision_Type = {(int) type.Key} AND {criteria.Period.FilterColumn(nameof(Contribution.Date))}")
                  .GroupBy("CrowdfunderRevision_Name", "CrowdfunderRevision_GoalsTotalBase", "CrowdfunderRevision_Url")
                  .OrderBy("ContributionsTotal DESC");

        LogQuery(sql);
        
        var rows = await db.FetchAsync<CrowdfunderRow>(sql);
        
        return rows;
    }
    
    private async Task<decimal> GetCompletedPercentageAsync(IUmbracoDatabase db,
                                                            CrowdfunderType type,
                                                            DashboardStatisticsCriteria criteria) {
        var latestRevisions = Sql.Builder
                                 .Select($"{nameof(CrowdfunderRevision.ContentKey)}, MAX({nameof(CrowdfunderRevision.ContentRevision)}) AS MaxRevision")
                                 .From($"{CrowdfundingConstants.Tables.CrowdfunderRevisions.Name}")
                                 .Where($"{nameof(CrowdfunderRevision.Type)} = {(int) type.Key}")
                                 .Append(criteria.Period.From.HasValue()
                                             ? $"AND {nameof(CrowdfunderRevision.ActiveFrom)} <= '{criteria.Period.To.GetValueOrThrow().ToYearMonthDayString()}'"
                                             : "")
                                 .Append(criteria.Period.To.HasValue()
                                             ? $"AND ({nameof(CrowdfunderRevision.ActiveTo)} IS NULL OR {nameof(CrowdfunderRevision.ActiveTo)} >= '{criteria.Period.From.GetValueOrThrow().ToYearMonthDayString()}')"
                                             : "")
                                 .GroupBy($"{nameof(CrowdfunderRevision.ContentKey)}");
        
        var crowdfunderGoalsSql = Sql.Builder
                                     .Select($"CR.{nameof(CrowdfunderRevision.ContentKey)}, SUM(CR.{nameof(CrowdfunderRevision.GoalsTotalBase)}) AS TotalGoals")
                                     .From($"{CrowdfundingConstants.Tables.CrowdfunderRevisions.Name} CR")
                                     .InnerJoin($"({latestRevisions.SQL}) AS LR") 
                                     .On($"CR.{nameof(CrowdfunderRevision.ContentKey)} = LR.{nameof(CrowdfunderRevision.ContentKey)} AND CR.{nameof(CrowdfunderRevision.ContentRevision)} = LR.MaxRevision")
                                     .Where($"CR.{nameof(CrowdfunderRevision.Type)} = {(int) type.Key}")
                                     .GroupBy($"CR.{nameof(CrowdfunderRevision.ContentKey)}");
        
        var crowdfunderContributionsSql = Sql.Builder
                                             .Select($"C.{nameof(Contribution.CrowdfunderId)}, SUM(C.{nameof(Contribution.BaseAmount)} + C.{nameof(Contribution.TaxReliefBaseAmount)}) AS TotalContributions")
                                             .From($"{CrowdfundingConstants.Tables.Contributions.Name} C")
                                             .InnerJoin($"({latestRevisions.SQL}) AS LR")
                                             .On($"C.{nameof(Contribution.CrowdfunderId)} = LR.{nameof(CrowdfunderRevision.ContentKey)}")
                                             .Where(criteria.Period.FilterColumn($"C.{nameof(Contribution.Date)}"))
                                             .GroupBy($"C.{nameof(Contribution.CrowdfunderId)}");

        var percentageCompletedSql = Sql.Builder
                                        .Select("AVG(CASE WHEN CG.TotalGoals > 0 THEN (CC.TotalContributions / CG.TotalGoals) * 100 ELSE 0 END) AS AveragePercentageComplete")
                                        .From($"({crowdfunderGoalsSql.SQL}) AS CG")
                                        .InnerJoin($"({crowdfunderContributionsSql.SQL}) AS CC")
                                        .On($"CG.{nameof(CrowdfunderRevision.ContentKey)} = CC.{nameof(Contribution.CrowdfunderId)}");
        
        LogQuery(percentageCompletedSql);
        
        var percentageComplete = await db.ExecuteScalarAsync<decimal>(percentageCompletedSql);

        return percentageComplete;
    }
    
    private class CrowdfunderRow {
        [Order(1)]
        public string Name { get; set; }
        
        [Order(2)]
        public decimal GoalsTotal { get; set; }
        
        [Order(3)]
        public decimal ContributionsTotal { get; set; }
        
        [Order(4)]
        public string Url { get; set; }
    }
}