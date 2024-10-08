using System.Linq;
using N3O.Umbraco.Crowdfunding.Criteria;
using N3O.Umbraco.Crowdfunding.Models;
using System.Threading.Tasks;
using N3O.Umbraco.Financial;
using NPoco;
using Umbraco.Cms.Infrastructure.Persistence;

namespace N3O.Umbraco.Crowdfunding.Handlers;

public partial class GetDashboardStatisticsHandler
{
    private async Task PopulateCampaignsAsync(IUmbracoDatabase db,
        DashboardStatisticsCriteria criteria,
        DashboardStatisticsRes res)
    {

        var totalActiveCampaignsSql = Sql.Builder
            .Select("COUNT(*) AS [Count]")
            .From("CrowdfunderRevisions")
            .Where("Type = 1")
            .Where("ActiveFrom <= @0", criteria.Period.To)
            .Where("(ActiveTo IS NULL OR ActiveTo >= @0)", criteria.Period.From);
        
            
        var sqlAveragePercentage = Sql.Builder
            .Select(@"AVG(CASE 
                      WHEN CG.TotalGoals > 0 THEN 
                          (CC.TotalContributions / CG.TotalGoals) * 100 
                      ELSE 0 
                  END) AS AveragePercentageComplete")
            .From(@"
        (SELECT 
            CR.Id, 
            SUM(CR.GoalsTotalBase) AS TotalGoals
         FROM 
            CrowdfunderRevisions CR
         INNER JOIN 
            (SELECT 
                Id, 
                MAX(Revision) AS MaxRevision
             FROM 
                CrowdfunderRevisions
             WHERE 
                Type = 1  -- Filter for campaigns only
                AND ActiveFrom <= @EndDate
                AND (ActiveTo IS NULL OR ActiveTo >= @StartDate)
             GROUP BY 
                Id) LR
         ON 
            CR.Id = LR.Id AND CR.Revision = LR.MaxRevision
         WHERE 
            CR.Type = 1  -- Filter for campaigns only
         GROUP BY 
            CR.Id) CG")  
            .InnerJoin(@"
        (SELECT 
            C.CampaignId, 
            SUM(C.BaseAmount + C.TaxReliefBaseAmount) AS TotalContributions
         FROM 
            Contributions C
         INNER JOIN 
            (SELECT 
                Id, 
                MAX(Revision) AS MaxRevision
             FROM 
                CrowdfunderRevisions
             WHERE 
                Type = 1  -- Filter for campaigns only
                AND ActiveFrom <= @EndDate
                AND (ActiveTo IS NULL OR ActiveTo >= @StartDate)
             GROUP BY 
                Id) LR
         ON 
            C.CampaignId = LR.Id
         INNER JOIN 
            CrowdfunderRevisions CR ON CR.Id = LR.Id AND CR.Revision = LR.MaxRevision
         WHERE 
            C.Date BETWEEN @StartDate AND @EndDate
         GROUP BY 
            C.CampaignId) CC")  
            .On("CG.Id = CC.CampaignId");
        
        
        var sqlTopTen = Sql.Builder
            .Select(@"TOP 10 CG.Name AS [Name], 
              CG.GoalsTotalBase AS [GoalsTotal], 
              SUM(C.BaseAmount + C.TaxReliefBaseAmount) AS [ContributionsTotal], 
              CG.Url AS [Url]")
            .From("Contributions C")
            .InnerJoin(@"(SELECT CR.Id, CR.Name, CR.GoalsTotalBase, CR.Url, 
                         ROW_NUMBER() OVER (PARTITION BY CR.Id ORDER BY CR.Revision DESC) AS RowNum
                  FROM CrowdfunderRevisions CR 
                  WHERE CR.Type = 1) CG")
            .On("C.CampaignId = CG.Id")
            .Where("CG.RowNum = 1 AND C.CampaignId IS NOT NULL AND C.Date BETWEEN @0 AND @1", criteria.Period.From,
                criteria.Period.To)
            .GroupBy("CG.Name, CG.GoalsTotalBase, CG.Url")
            .OrderBy("ContributionsTotal DESC");
        
        
        var rawResults = await db.FetchAsync<dynamic>(sqlTopTen);

        var TopTen = rawResults.Select(result => new CrowdfunderStatisticsItemRes
        {
            Name = result.Name,
            GoalsTotal = new MoneyRes
            {
                Amount = result.GoalsTotal ?? 0,
                Text = $"{result.GoalsTotal ?? 0}"
            },
            ContributionsTotal = new MoneyRes
            {
                Amount = result.ContributionsTotal ?? 0,
                Text = $"{result.ContributionsTotal ?? 0}"
            },
            Url = result.Url,
        }).ToList();

        // Create a new CampaignStatisticsRes instance and assign top items
        res.Campaigns = new CampaignStatisticsRes
        {
            TopItems = TopTen
        };
        res.Campaigns.Count = await db.ExecuteScalarAsync<int>(totalActiveCampaignsSql);
        res.Campaigns.AveragePercentageComplete = await db.ExecuteScalarAsync<decimal>(sqlAveragePercentage);
        
        await PopulateCrowdfunderAsync(db, criteria, res.Campaigns);

    }
}