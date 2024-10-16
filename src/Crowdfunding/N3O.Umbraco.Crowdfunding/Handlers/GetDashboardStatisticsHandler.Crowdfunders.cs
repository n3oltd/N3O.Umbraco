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
        var topTenCrowdfunders = await GetTopCrowdfundersAsync(db, type, criteria);
        var activeCrowdfundersCount = await GetActiveCrowdfundersCountAsync(db, type, criteria);
        var completedPercentage = await GetCompletedPercentageAsync(db, criteria);
        
        res.Count = activeCrowdfundersCount;
        res.AveragePercentageComplete = completedPercentage;
        res.TopItems = topTenCrowdfunders.Select(x => new CrowdfunderStatisticsItemRes {
            Name = x.Name,
            GoalsTotal = GetMoneyRes(x.GoalsTotal),
            ContributionsTotal = GetMoneyRes(x.ContributionsTotal),
            Url = x.Url
        });
    }

    private async Task<int> GetActiveCrowdfundersCountAsync(IUmbracoDatabase db,
                                                            CrowdfunderType type,
                                                            DashboardStatisticsCriteria criteria) {
        var totalActiveCampaignsQuery = Sql.Builder
                                           .Select($"COUNT(*) AS {nameof(CampaignStatisticsRow.Count)}")
                                           .From($"{CrowdfundingConstants.Tables.CrowdfunderRevisions.Name}")
                                           .Where($"{nameof(CrowdfunderRevision.Type)} = {(int) type.Key}")
                                           .Where($"{nameof(CrowdfunderRevision.ActiveFrom)} <= @0", criteria.Period.From.Value.ToDateTimeUnspecified())
                                           .Append($"AND ({nameof(CrowdfunderRevision.ActiveTo)} IS NULL OR {nameof(CrowdfunderRevision.ActiveTo)} >= @0)", criteria.Period.From.Value.ToDateTimeUnspecified());
        
        var count = await db.ExecuteScalarAsync<int>(totalActiveCampaignsQuery);
        
        return count;
    }

    private async Task<IReadOnlyList<CrowdfunderRow>> GetTopCrowdfundersAsync(IUmbracoDatabase db,
                                                                              CrowdfunderType type,
                                                                              DashboardStatisticsCriteria criteria) {
        var crowdfunderRevisionQuery = Sql.Builder
                                          .Select("(FR.Id, FR.Name, FR.GoalsTotalBase, FR.Url, ROW_NUMBER() OVER (PARTITION BY FR.Id ORDER BY FR.Revision DESC) AS RowNum")
                                          .From($"{CrowdfundingConstants.Tables.CrowdfunderRevisions.Name} FR ")
                                          .Where($"FR.Type = {(int) type.Key}) FG");
        
        var sqlTopTenFundraisers = Sql.Builder
                                      .Select(@"TOP (10) FG.Name AS [Name], FG.GoalsTotalBase AS [GoalsTotal], SUM(F.BaseAmount + F.TaxReliefBaseAmount) AS [ContributionsTotal], FG.Url AS [Url]")
                                      .From($"{CrowdfundingConstants.Tables.Contributions.Name} F")
                                      .InnerJoin($"({crowdfunderRevisionQuery.SQL})")  
                                      .On("F.FundraiserId = FG.Id")
                                      .Where("FG.RowNum = 1 AND F.FundraiserId IS NOT NULL AND F.Date BETWEEN @0 AND @1", criteria.Period.From, criteria.Period.To)
                                      .GroupBy("FG.Name, FG.GoalsTotalBase, FG.Url")
                                      .OrderBy("ContributionsTotal DESC");
        
        var rows = await db.FetchAsync<CrowdfunderRow>(sqlTopTenFundraisers);
        
        return rows;
    }
    
    private async Task<decimal> GetCompletedPercentageAsync(IUmbracoDatabase db, DashboardStatisticsCriteria criteria) {
        var latestRevisionsQuery = Sql.Builder
                                      .Select($"{nameof(CrowdfunderRevision.ContentKey)}, MAX({nameof(CrowdfunderRevision.ContentRevision)}) AS {nameof(CrowdfunderRevision.ContentRevision)}")
                                      .From($"{CrowdfundingConstants.Tables.CrowdfunderRevisions.Name}")
                                      .Where($"{nameof(CrowdfunderRevision.Type)} = {(int) CrowdfunderTypes.Campaign.Key}")
                                      .Where($"{nameof(CrowdfunderRevision.ActiveFrom)} <= {criteria.Period.From.Value.ToDateTimeUnspecified()}")
                                      .Append($"AND ({nameof(CrowdfunderRevision.ActiveFrom)} IS NULL OR {nameof(CrowdfunderRevision.ActiveFrom)} >= {criteria.Period.To.Value.ToDateTimeUnspecified()})")
                                      .GroupBy($"{nameof(CrowdfunderRevision.Type)} = {(int) CrowdfunderTypes.Campaign.Key}");
        
        var totalGoalsQuery = Sql.Builder
                                 .Select($"CR.{nameof(CrowdfunderRevision.ContentKey)}, SUM(CR.{nameof(CrowdfunderRevision.GoalsTotalBase)}) AS {nameof(CrowdfunderRevision.GoalsTotalBase)}")
                                 .From($"{CrowdfundingConstants.Tables.CrowdfunderRevisions.Name} CR")
                                 .InnerJoin($"({latestRevisionsQuery.SQL}) AS LR") 
                                 .On($"CR.{nameof(CrowdfunderRevision.ContentKey)} = LR.{nameof(CrowdfunderRevision.ContentKey)} AND CR.{nameof(CrowdfunderRevision.ContentRevision)} = LR.{nameof(CrowdfunderRevision.ContentRevision)}")
                                 .Where($"CR.{nameof(CrowdfunderRevision.Type)} = {(int) CrowdfunderTypes.Campaign.Key}")
                                 .GroupBy($"CR.{nameof(CrowdfunderRevision.Type)} = {(int) CrowdfunderTypes.Campaign.Key}");
        
        var totalContributionsQuery = Sql.Builder
                                         .Select("C.CampaignId, SUM(C.BaseAmount + C.TaxReliefBaseAmount) AS TotalContributions")
                                         .From("Contributions C")
                                         .InnerJoin($"({latestRevisionsQuery.SQL}) AS LR")
                                         .On("C.CampaignId = LR.Id")
                                         .InnerJoin("CrowdfunderRevisions CR")
                                         .On(" CR.Id = LR.Id AND CR.Revision = LR.MaxRevision")
                                         .Where($"C.Date BETWEEN {criteria.Period.From.Value.ToDateTimeUnspecified()} AND {criteria.Period.From.Value.ToDateTimeUnspecified()}")
                                         .GroupBy("C.CampaignId");
        
        var completedPercentages = Sql.Builder
                                      .Select("AVG(CASE WHEN CG.TotalGoals > 0 THEN (CC.TotalContributions / CG.TotalGoals) * 100 ELSE 0 END) AS AveragePercentageComplete")
                                      .From($"({totalGoalsQuery.SQL}) AS CG")
                                      .InnerJoin($"({totalContributionsQuery.SQL}) AS CC")
                                      .On("CG.Id = CC.CampaignId"); 
        
        var averagePercentageComplete = await db.ExecuteScalarAsync<decimal>(completedPercentages);

        return averagePercentageComplete;
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