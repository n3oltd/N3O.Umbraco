using N3O.Umbraco.Attributes;
using N3O.Umbraco.Crm.Lookups;
using N3O.Umbraco.Crowdfunding.Criteria;
using N3O.Umbraco.Crowdfunding.Entities;
using N3O.Umbraco.Crowdfunding.Models;
using System.Threading.Tasks;
using NPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Cms.Infrastructure.Persistence;

namespace N3O.Umbraco.Crowdfunding.Handlers;

public partial class GetDashboardStatisticsHandler {
    private async Task PopulateFundraisersAsync(IUmbracoDatabase db,
                                                DashboardStatisticsCriteria criteria,
                                                DashboardStatisticsRes res) {
        var from = criteria.Period?.From?.ToDateTimeUnspecified();
        var to = criteria.Period?.To?.ToDateTimeUnspecified();
        
        res.Fundraisers = new FundraiserStatisticsRes(); 
        
        var fundraisersByCampaign = await GetFundraisersByCampaignAsync(db, from, to);
        var newFundraiser = await GetNewFundraisersAsync(db, from, to);
        var completedFundraiser = await GetCompletedFundraisersAsync(db, from, to);
        
        await PopulateCrowdfunderAsync(db, CrowdfunderTypes.Fundraiser, criteria, res.Fundraisers);
        
        res.Fundraisers.ActiveCount = res.Fundraisers.ActiveCount;
        res.Fundraisers.NewCount = newFundraiser;
        res.Fundraisers.CompletedCount = completedFundraiser;
        res.Fundraisers.ByCampaign = fundraisersByCampaign.Select(x => new FundraiserByCampaignStatisticsRes {
                                                                                 CampaignName = x.CampaignName,
                                                                                 Count = x.FundraiserCount
                                                                             });
    }

    private async Task<int> GetNewFundraisersAsync(IUmbracoDatabase db, DateTime? from, DateTime? to) {
        var newFundraisers = Sql.Builder
                                .Select("COUNT(*) AS NewCount")
                                .From($"{CrowdfundingConstants.Tables.CrowdfunderRevisions.Name}")
                                .Where($"{nameof(CrowdfunderRevision.Type)} = {(int) CrowdfunderTypes.Fundraiser.Key} AND {nameof(CrowdfunderRevision.ActiveFrom)} BETWEEN '{from}' AND '{to}'");
        
        var count = await db.ExecuteScalarAsync<int>(newFundraisers);
        
        return count;
    }
    
    private async Task<int> GetCompletedFundraisersAsync(IUmbracoDatabase db, DateTime? from, DateTime? to) {
        var completedFundraisers = Sql.Builder
                              .Select("COUNT(*) AS CompletedCount")
                              .From($"{CrowdfundingConstants.Tables.CrowdfunderRevisions.Name}")
                              .Where($"{nameof(CrowdfunderRevision.Type)} = {(int) CrowdfunderTypes.Fundraiser.Key} AND {nameof(CrowdfunderRevision.GoalCompletedOn)} BETWEEN '{from}' AND '{to}'");
        
        var count = await db.ExecuteScalarAsync<int>(completedFundraisers);
        
        return count;
    }

    private async Task<IReadOnlyList<FundraisersByCampaignRow>> GetFundraisersByCampaignAsync(IUmbracoDatabase db,
                                                                                              DateTime? from,
                                                                                              DateTime? to) {
        var allFundraisers = Sql.Builder
                                .Select($"{nameof(CrowdfunderRevision.CampaignId)}")
                                .From($"{CrowdfundingConstants.Tables.CrowdfunderRevisions.Name}")
                                .Where($"{nameof(CrowdfunderRevision.Type)} = {(int) CrowdfunderTypes.Fundraiser.Key} AND {nameof(CrowdfunderRevision.ActiveFrom)} <= '{to}'")
                                .Append($"AND ({nameof(CrowdfunderRevision.ActiveTo)} IS NULL OR {nameof(CrowdfunderRevision.ActiveTo)} >= '{from}')");

        var allCampaigns = Sql.Builder
                              .Select($"{nameof(CrowdfunderRevision.Name)} AS CampaignName, {nameof(CrowdfunderRevision.ContentKey)} AS CampaignId")
                              .From($"{CrowdfundingConstants.Tables.CrowdfunderRevisions.Name}")
                              .Where($"{nameof(CrowdfunderRevision.Type)} = {(int) CrowdfunderTypes.Campaign.Key} AND {nameof(CrowdfunderRevision.ActiveFrom)} <= '{to}'")
                              .Append($"AND ({nameof(CrowdfunderRevision.ActiveTo)} IS NULL OR {nameof(CrowdfunderRevision.ActiveTo)} >= '{from}')");
        
        var allFundraisersByCampaign = Sql.Builder
                                          .Select($"Campaigns.CampaignName AS {nameof(FundraisersByCampaignRow.CampaignName)}, COUNT(Fundraisers.CampaignId) AS {nameof(FundraisersByCampaignRow.FundraiserCount)}")
                                          .From($"({allCampaigns.SQL}) AS Campaigns")
                                          .LeftJoin($"({allFundraisers.SQL}) AS Fundraisers")
                                          .On("Campaigns.CampaignId = Fundraisers.CampaignId")
                                          .GroupBy("Campaigns.CampaignName");
        
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