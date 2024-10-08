using System.Linq;
using N3O.Umbraco.Crowdfunding.Criteria;
using N3O.Umbraco.Crowdfunding.Models;
using System.Threading.Tasks;
using N3O.Umbraco.Financial;
using N3O.Umbraco.References;
using NPoco;
using Umbraco.Cms.Infrastructure.Persistence;

namespace N3O.Umbraco.Crowdfunding.Handlers;

public partial class GetDashboardStatisticsHandler {
    private async Task PopulateFundraisersAsync(IUmbracoDatabase db,
                                                DashboardStatisticsCriteria criteria,
                                                DashboardStatisticsRes res) {
        //queries for the crowdfunders panel
        var totalActiveFundraisersSql = Sql.Builder
            .Select("COUNT(*) AS [Count]")
            .From("CrowdfunderRevisions")
            .Where("Type = 2") 
            .Where("ActiveFrom <= @0", criteria.Period.To)
            .Where("(ActiveTo IS NULL OR ActiveTo >= @0)", criteria.Period.From);

        
        // SQL query to calculate the average completion percentage for fundraisers
        var sqlAveragePercentage = Sql.Builder
            .Select(@"AVG(CASE 
                          WHEN FG.TotalGoals > 0 THEN 
                              (FC.TotalContributions / FG.TotalGoals) * 100 
                          ELSE 0 
                      END) AS AveragePercentageComplete")
            .From(@"
                (SELECT 
                    FR.Id, 
                    SUM(FR.GoalsTotalBase) AS TotalGoals
                 FROM 
                    CrowdfunderRevisions FR
                 INNER JOIN 
                    (SELECT 
                        Id, 
                        MAX(Revision) AS MaxRevision
                     FROM 
                        CrowdfunderRevisions
                     WHERE 
                        Type = 2  -- Filter for fundraisers only
                        AND ActiveFrom <= @EndDate
                        AND (ActiveTo IS NULL OR ActiveTo >= @StartDate)
                     GROUP BY 
                        Id) LR
                 ON 
                    FR.Id = LR.Id AND FR.Revision = LR.MaxRevision
                 WHERE 
                    FR.Type = 2  -- Filter for fundraisers only
                 GROUP BY 
                    FR.Id) FG")  
            .InnerJoin(@"
                (SELECT 
                    F.FundraiserId, 
                    SUM(F.BaseAmount + F.TaxReliefBaseAmount) AS TotalContributions
                 FROM 
                    Contributions F
                 INNER JOIN 
                    (SELECT 
                        Id, 
                        MAX(Revision) AS MaxRevision
                     FROM 
                        CrowdfunderRevisions
                     WHERE 
                        Type = 2  -- Filter for fundraisers only
                        AND ActiveFrom <= @EndDate
                        AND (ActiveTo IS NULL OR ActiveTo >= @StartDate)
                     GROUP BY 
                        Id) LR
                 ON 
                    F.FundraiserId = LR.Id
                 INNER JOIN 
                    CrowdfunderRevisions FR ON FR.Id = LR.Id AND FR.Revision = LR.MaxRevision
                 WHERE 
                    F.Date BETWEEN @StartDate AND @EndDate
                 GROUP BY 
                    F.FundraiserId) FC")  
            .On("FG.Id = FC.FundraiserId");

        
        // SQL query to retrieve the top 10 fundraisers
        var sqlTopTenFundraisers = Sql.Builder
            .Select(@"TOP 10 FG.Name AS [Name], 
                      FG.GoalsTotalBase AS [GoalsTotal], 
                      SUM(F.BaseAmount + F.TaxReliefBaseAmount) AS [ContributionsTotal], 
                      FG.Url AS [Url]")
            .From("Contributions F")
            .InnerJoin(@"(SELECT FR.Id, FR.Name, FR.GoalsTotalBase, FR.Url, 
                                 ROW_NUMBER() OVER (PARTITION BY FR.Id ORDER BY FR.Revision DESC) AS RowNum
                          FROM CrowdfunderRevisions FR 
                          WHERE FR.Type = 2) FG")  
            .On("F.FundraiserId = FG.Id")
            .Where("FG.RowNum = 1 AND F.FundraiserId IS NOT NULL AND F.Date BETWEEN @0 AND @1", criteria.Period.From,
                criteria.Period.To)
            .GroupBy("FG.Name, FG.GoalsTotalBase, FG.Url")
            .OrderBy("ContributionsTotal DESC");
        
        
        
        //queries for fundraiser stats
        var sqlActive = Sql.Builder
            .Select("COUNT(*) AS [ActiveCount]")
            .From("CrowdfunderRevisions")
            .Where("Type = 2") 
            .Where("ActiveFrom <= @0", criteria.Period.To)
            .Where("(ActiveTo IS NULL OR ActiveTo >= @0)", criteria.Period.From);
                
        var sqlNew = Sql.Builder
            .Select("COUNT(*) AS NewCount")
            .From("Contributions")
            .Where("FundraiserID IS NOT NULL AND Date BETWEEN @0 AND @1", criteria.Period.From, criteria.Period.To);

        
        var sqlCompleted = Sql.Builder
            .Select("COUNT(*) AS CompletedCount")
            .From("Contributions")
            .Where("FundraiserID IS NOT NULL AND Status = 'Completed' AND Date BETWEEN @0 AND @1", criteria.Period.From, criteria.Period.To);

        
        var sqlFundraisersbyCampaign = Sql.Builder
            .Select("CampaignName, COUNT(*) AS FundraiserCount")
            .From("Contributions")
            .Where("FundraiserId IS NOT NULL AND Date BETWEEN @0 AND @1", criteria.Period.From, criteria.Period.To)
            .GroupBy("CampaignName");

        var FundraisersByCampaignres = await db.FetchAsync<dynamic>(sqlFundraisersbyCampaign);

        var fundraisersByCampaign = FundraisersByCampaignres.Select(result => new FundraiserByCampaignStatisticsRes {
            CampaignName = result.CampaignName,
            Count = result.FundraiserCount
        }).ToList();
        
        var TopTenFundraisersres = await db.FetchAsync<dynamic>(sqlTopTenFundraisers);

        var TopTenFundraisers = TopTenFundraisersres.Select(result => new CrowdfunderStatisticsItemRes
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
        
        
        //for fundraiser statistics
        res.Fundraisers = new FundraiserStatisticsRes();
        res.Fundraisers.ActiveCount = await db.ExecuteScalarAsync<int>(sqlActive);
        res.Fundraisers.NewCount = await db.ExecuteScalarAsync<int>(sqlNew);
        res.Fundraisers.CompletedCount = await db.ExecuteScalarAsync<int>(sqlCompleted);
        res.Fundraisers.ByCampaign = fundraisersByCampaign;

        //for crowdfunder statistics
        res.Fundraisers.TopItems = TopTenFundraisers;
        res.Fundraisers.Count = await db.ExecuteScalarAsync<int>(totalActiveFundraisersSql);
        res.Fundraisers.AveragePercentageComplete = await db.ExecuteScalarAsync<decimal>(sqlAveragePercentage);
        
        await PopulateCrowdfunderAsync(db, criteria, res.Fundraisers);
    }
}