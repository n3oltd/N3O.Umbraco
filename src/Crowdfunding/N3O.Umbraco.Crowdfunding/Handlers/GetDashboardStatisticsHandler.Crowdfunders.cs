using N3O.Umbraco.Attributes;
using N3O.Umbraco.Crm.Lookups;
using N3O.Umbraco.Crowdfunding.Criteria;
using N3O.Umbraco.Crowdfunding.Entities;
using N3O.Umbraco.Crowdfunding.Models;
using NPoco;
using System;
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
        var from = criteria.Period?.From?.ToDateTimeUnspecified();
        var to = criteria.Period?.To?.ToDateTimeUnspecified();
        
        var topCrowdfunders = await GetTopCrowdfunderRowsAsync(db, type, from, to);
        var activeCrowdfundersCount = await GetActiveCrowdfundersCountAsync(db, type, from, to);
        var completedPercentage = await GetCompletedPercentageAsync(db, from, to);
        
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
                                                            DateTime? from,
                                                            DateTime? to) {
        var totalActiveCampaignsQuery = Sql.Builder
                                           .Select("COUNT(*)")
                                           .From($"{CrowdfundingConstants.Tables.CrowdfunderRevisions.Name}")
                                           .Where($"{nameof(CrowdfunderRevision.Type)} = {(int) type.Key}")
                                           .Append($"AND {nameof(CrowdfunderRevision.ActiveFrom)} <= '{to}'")
                                           .Append($"AND ({nameof(CrowdfunderRevision.ActiveTo)} IS NULL OR {nameof(CrowdfunderRevision.ActiveTo)} >= '{from}')");
        
        var count = await db.ExecuteScalarAsync<int>(totalActiveCampaignsQuery);
        
        return count;
    }

    private async Task<IReadOnlyList<CrowdfunderRow>> GetTopCrowdfunderRowsAsync(IUmbracoDatabase db,
                                                                                 CrowdfunderType type,
                                                                                 DateTime? from,
                                                                                 DateTime? to) {
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
                 .Where($"RowNum = 1 AND CrowdfunderRevision_Type = {(int) type.Key} AND {nameof(Contribution.Date)} BETWEEN '{from}' AND '{to}'")
                 .GroupBy("CrowdfunderRevision_Name", "CrowdfunderRevision_GoalsTotalBase", "CrowdfunderRevision_Url")
                 .OrderBy("ContributionsTotal DESC");
        
        var rows = await db.FetchAsync<CrowdfunderRow>(sql);
        
        return rows;
    }
    
    private async Task<decimal> GetCompletedPercentageAsync(IUmbracoDatabase db, DateTime? from, DateTime? to) {
        var latestRevisions = Sql.Builder
                                 .Select($"{nameof(CrowdfunderRevision.ContentKey)}, MAX({nameof(CrowdfunderRevision.ContentRevision)}) AS MaxRevision")
                                 .From($"{CrowdfundingConstants.Tables.CrowdfunderRevisions.Name}")
                                 .Where($"{nameof(CrowdfunderRevision.Type)} = {(int) CrowdfunderTypes.Campaign.Key} AND {nameof(CrowdfunderRevision.ActiveFrom)} <= '{to}'")
                                 .Append($"AND ({nameof(CrowdfunderRevision.ActiveTo)} IS NULL OR {nameof(CrowdfunderRevision.ActiveTo)} >= '{from}')")
                                 .GroupBy($"{nameof(CrowdfunderRevision.ContentKey)}");
        
        var crowdfunderGoalsSql = Sql.Builder
                                     .Select($"CR.{nameof(CrowdfunderRevision.ContentKey)}, SUM(CR.{nameof(CrowdfunderRevision.GoalsTotalBase)}) AS TotalGoals")
                                     .From($"{CrowdfundingConstants.Tables.CrowdfunderRevisions.Name} CR")
                                     .InnerJoin($"({latestRevisions.SQL}) AS LR") 
                                     .On($"CR.{nameof(CrowdfunderRevision.ContentKey)} = LR.{nameof(CrowdfunderRevision.ContentKey)} AND CR.{nameof(CrowdfunderRevision.ContentRevision)} = LR.MaxRevision")
                                     .Where($"CR.{nameof(CrowdfunderRevision.Type)} = {(int) CrowdfunderTypes.Campaign.Key}")
                                     .GroupBy($"CR.{nameof(CrowdfunderRevision.ContentKey)}");
        
        var campaignContributionsSql = Sql.Builder
                                          .Select($"C.{nameof(Contribution.CrowdfunderId)}, SUM(C.{nameof(Contribution.BaseAmount)} + C.{nameof(Contribution.TaxReliefBaseAmount)}) AS TotalContributions")
                                          .From($"{CrowdfundingConstants.Tables.Contributions.Name} C")
                                          .InnerJoin($"({latestRevisions.SQL}) AS LR")
                                          .On($"C.{nameof(Contribution.CrowdfunderId)} = LR.{nameof(CrowdfunderRevision.ContentKey)}")
                                          .Where($"C.{nameof(Contribution.Date)} BETWEEN '{from}' AND '{to}'")
                                          .GroupBy($"C.{nameof(Contribution.CrowdfunderId)}");
        
        var percentageCompletedSql = Sql.Builder
                                      .Select("AVG(CASE WHEN CG.TotalGoals > 0 THEN (CC.TotalContributions / CG.TotalGoals) * 100 ELSE 0 END) AS AveragePercentageComplete")
                                      .From($"({crowdfunderGoalsSql.SQL}) AS CG")
                                      .InnerJoin($"({campaignContributionsSql.SQL}) AS CC")
                                      .On($"CG.{nameof(CrowdfunderRevision.ContentKey)} = CC.{nameof(Contribution.CrowdfunderId)}");
        
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