using N3O.Umbraco.Crm.Lookups;
using System.Linq;
using N3O.Umbraco.Crowdfunding.Criteria;
using N3O.Umbraco.Crowdfunding.Entities;
using N3O.Umbraco.Crowdfunding.Models;
using System.Threading.Tasks;
using N3O.Umbraco.Financial;
using NPoco;
using Umbraco.Cms.Infrastructure.Persistence;

namespace N3O.Umbraco.Crowdfunding.Handlers;

public partial class GetDashboardStatisticsHandler {
    private async Task PopulateFundraisersAsync(IUmbracoDatabase db,
                                                DashboardStatisticsCriteria criteria,
                                                DashboardStatisticsRes res) {
        //queries for fundraiser stats
        var activeFundraisers = Sql.Builder
                                   .Select("COUNT(*)")
                                   .From($"{CrowdfundingConstants.Tables.CrowdfunderRevisions.Name}")
                                   .Where($"{nameof(CrowdfunderRevision.Type)} = {(int) CrowdfunderTypes.Fundraiser.Key}") 
                                   .Where($"{nameof(CrowdfunderRevision.ActiveFrom)} <= @0", criteria.Period.To)
                                   .Where($"({nameof(CrowdfunderRevision.ActiveTo)} IS NULL OR {nameof(CrowdfunderRevision.ActiveTo)} >= @0)", criteria.Period.From);
                
        //TODO Wrong Table
        var sqlNew = Sql.Builder
                        .Select("COUNT(*) AS NewCount")
                        .From("Contributions")
                        .Where("FundraiserID IS NOT NULL AND Date BETWEEN @0 AND @1", criteria.Period.From, criteria.Period.To);

        
        //TODO Wrong Table
        var sqlCompleted = Sql.Builder
                              .Select("COUNT(*) AS CompletedCount")
                              .From("Contributions")
                              .Where("FundraiserID IS NOT NULL AND Status = 'Completed' AND Date BETWEEN @0 AND @1", criteria.Period.From, criteria.Period.To);

        
        //TODO Wrong Table
        var sqlFundraisersbyCampaign = Sql.Builder
                                          .Select("CampaignName, COUNT(*) AS FundraiserCount")
                                          .From($"{CrowdfundingConstants.Tables.CrowdfunderRevisions.Name}")
                                          .Where("FundraiserId IS NOT NULL AND Date BETWEEN @0 AND @1", criteria.Period.From, criteria.Period.To)
                                          .GroupBy("CampaignName");

        var FundraisersByCampaignres = await db.FetchAsync<dynamic>(sqlFundraisersbyCampaign);

        var fundraisersByCampaign = FundraisersByCampaignres.Select(result => new FundraiserByCampaignStatisticsRes {
            CampaignName = result.CampaignName,
            Count = result.FundraiserCount
        }).ToList();
        
        
        //for fundraiser statistics
        res.Fundraisers = new FundraiserStatisticsRes();
        res.Fundraisers.ActiveCount = await db.ExecuteScalarAsync<int>(activeFundraisers);
        res.Fundraisers.NewCount = await db.ExecuteScalarAsync<int>(sqlNew);
        res.Fundraisers.CompletedCount = await db.ExecuteScalarAsync<int>(sqlCompleted);
        res.Fundraisers.ByCampaign = fundraisersByCampaign;
        
        await PopulateCrowdfunderAsync(db, criteria, res.Fundraisers);
    }
}