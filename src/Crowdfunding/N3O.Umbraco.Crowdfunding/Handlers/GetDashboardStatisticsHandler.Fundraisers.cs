using N3O.Umbraco.Attributes;
using N3O.Umbraco.Crm.Lookups;
using N3O.Umbraco.Crowdfunding.Criteria;
using N3O.Umbraco.Crowdfunding.Entities;
using N3O.Umbraco.Crowdfunding.Extensions;
using N3O.Umbraco.Crowdfunding.Models;
using N3O.Umbraco.Extensions;
using System.Threading.Tasks;
using NPoco;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Cms.Infrastructure.Persistence;

namespace N3O.Umbraco.Crowdfunding.Handlers;

public partial class GetDashboardStatisticsHandler {
    private async Task PopulateFundraisersAsync(IUmbracoDatabase db,
                                                DashboardStatisticsCriteria criteria,
                                                DashboardStatisticsRes res) {
        res.Fundraisers = new FundraiserStatisticsRes(); 
        
        var fundraisersByCampaign = await GetFundraisersByCampaignAsync(db, criteria);
        var newFundraiser = await GetNewFundraisersAsync(db, criteria);
        var completedFundraiser = await GetCompletedFundraisersAsync(db, criteria);
        
        await PopulateCrowdfunderAsync(db, CrowdfunderTypes.Fundraiser, criteria, res.Fundraisers);
        
        res.Fundraisers.ActiveCount = res.Fundraisers.Count;
        res.Fundraisers.NewCount = newFundraiser;
        res.Fundraisers.CompletedCount = completedFundraiser;
        res.Fundraisers.ByCampaign = fundraisersByCampaign.Select(x => new FundraiserByCampaignStatisticsRes {
                                                                                 CampaignName = x.CampaignName,
                                                                                 Count = x.FundraiserCount
                                                                             });
    }

    private async Task<int> GetNewFundraisersAsync(IUmbracoDatabase db, DashboardStatisticsCriteria criteria) {
        var newFundraisers = Sql.Builder
                                .Select($"COUNT(DISTINCT {nameof(CrowdfunderRevision.ContentKey)})")
                                .From($"{CrowdfundingConstants.Tables.CrowdfunderRevisions.Name}")
                                .Where($"{nameof(CrowdfunderRevision.Type)} = {(int) CrowdfunderTypes.Fundraiser.Key} AND {criteria.Period.FilterColumn(nameof(CrowdfunderRevision.ActiveFrom))}");

        LogQuery(newFundraisers);
        
        var count = await db.ExecuteScalarAsync<int>(newFundraisers);
        
        return count;
    }
    
    private async Task<int> GetCompletedFundraisersAsync(IUmbracoDatabase db, DashboardStatisticsCriteria criteria) {
        var completedFundraisers = Sql.Builder
                                      .Select("COUNT(*) AS CompletedCount")
                                      .From($"{CrowdfundingConstants.Tables.CrowdfunderRevisions.Name}")
                                      .Where($"{nameof(CrowdfunderRevision.Type)} = {(int) CrowdfunderTypes.Fundraiser.Key} AND {criteria.Period.FilterColumn(nameof(CrowdfunderRevision.GoalCompletedOn))}");
        
        LogQuery(completedFundraisers);
        
        var count = await db.ExecuteScalarAsync<int>(completedFundraisers);
        
        return count;
    }

    private async Task<IReadOnlyList<FundraisersByCampaignRow>> GetFundraisersByCampaignAsync(IUmbracoDatabase db,
                                                                                              DashboardStatisticsCriteria criteria) {
        var allFundraisers = Sql.Builder
                                .Select($"{nameof(CrowdfunderRevision.CampaignId)}")
                                .From($"{CrowdfundingConstants.Tables.CrowdfunderRevisions.Name}")
                                .Where($"{nameof(CrowdfunderRevision.Type)} = {(int) CrowdfunderTypes.Fundraiser.Key}")
                                .Append(criteria.Period.From.HasValue()
                                            ? $"AND {nameof(CrowdfunderRevision.ActiveFrom)} <= '{criteria.Period.To.GetValueOrThrow().ToYearMonthDayString()}'"
                                            : "")
                                .Append(criteria.Period.To.HasValue()
                                            ? $"AND ({nameof(CrowdfunderRevision.ActiveTo)} IS NULL OR {nameof(CrowdfunderRevision.ActiveTo)} >= '{criteria.Period.From.GetValueOrThrow().ToYearMonthDayString()}')"
                                            : "");

        var allCampaigns = Sql.Builder
                              .Select($"{nameof(CrowdfunderRevision.Name)} AS CampaignName, {nameof(CrowdfunderRevision.ContentKey)} AS CampaignId")
                              .From($"{CrowdfundingConstants.Tables.CrowdfunderRevisions.Name}")
                              .Where($"{nameof(CrowdfunderRevision.Type)} = {(int) CrowdfunderTypes.Campaign.Key}")
                              .Append(criteria.Period.From.HasValue()
                                          ? $"AND {nameof(CrowdfunderRevision.ActiveFrom)} <= '{criteria.Period.To.GetValueOrThrow().ToYearMonthDayString()}'"
                                          : "")
                              .Append(criteria.Period.To.HasValue()
                                          ? $"AND ({nameof(CrowdfunderRevision.ActiveTo)} IS NULL OR {nameof(CrowdfunderRevision.ActiveTo)} >= '{criteria.Period.From.GetValueOrThrow().ToYearMonthDayString()}')"
                                          : "");
        
        var allFundraisersByCampaign = Sql.Builder
                                          .Select($"Campaigns.CampaignName AS {nameof(FundraisersByCampaignRow.CampaignName)}, COUNT(Fundraisers.CampaignId) AS {nameof(FundraisersByCampaignRow.FundraiserCount)}")
                                          .From($"({allCampaigns.SQL}) AS Campaigns")
                                          .LeftJoin($"({allFundraisers.SQL}) AS Fundraisers")
                                          .On("Campaigns.CampaignId = Fundraisers.CampaignId")
                                          .GroupBy("Campaigns.CampaignName");
        
        LogQuery(allFundraisersByCampaign);
        
        var fundraisersByCampaign = await db.FetchAsync<FundraisersByCampaignRow>(allFundraisersByCampaign);

        return fundraisersByCampaign;
    }
    
    private class FundraisersByCampaignRow {
        [Order(1)]
        public string CampaignName { get; set; }
        
        [Order(2)]
        public int FundraiserCount { get; set; }
    }
}