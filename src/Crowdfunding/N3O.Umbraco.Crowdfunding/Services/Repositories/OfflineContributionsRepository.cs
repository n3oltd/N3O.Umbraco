using N3O.Umbraco.Crowdfunding;
using N3O.Umbraco.Crowdfunding.Entities;
using N3O.Umbraco.Crowdfunding.Models;
using N3O.Umbraco.Extensions;
using NPoco;
using System;
using System.Threading.Tasks;
using Umbraco.Cms.Infrastructure.Persistence;

namespace N3O.Umbraco.CrowdFunding.Services;

public class OfflineContributionsRepository : IOfflineContributionsRepository {
    private readonly IUmbracoDatabaseFactory _umbracoDatabaseFactory;

    public OfflineContributionsRepository(IUmbracoDatabaseFactory umbracoDatabaseFactory) {
        _umbracoDatabaseFactory = umbracoDatabaseFactory;
    }

    public async Task AddOrUpdateAsync(WebhookPledge pledge) {
        var (isNew, offlineContributions) = await GetCrowdfundingOfflineContributionsAsync(pledge);
        
        if(offlineContributions.PledgeRevision >= pledge.Revision.Number){
            return;
        }

        using var db = _umbracoDatabaseFactory.CreateDatabase();

        if (isNew) {
            await db.InsertAsync(offlineContributions);
        } else {
            await db.UpdateAsync(offlineContributions);
        }
    }

    public async Task<CrowdfundingOfflineContributions> FindByCampaignAsync(Guid campaignId) {
        return await FindContributionAsync(Sql.Builder.Where($"{nameof(CrowdfundingOfflineContributions.CampaignId)} = {campaignId} "));
    }

    public async Task<CrowdfundingOfflineContributions> FindByFundraiserAsync(Guid fundraiserId) {
        return await FindContributionAsync(Sql.Builder.Where($"{nameof(CrowdfundingOfflineContributions.FundraiserId)} = {fundraiserId}"));
    }

    private async Task<CrowdfundingOfflineContributions> FindContributionAsync(Sql whereClause) {
        using (var db = _umbracoDatabaseFactory.CreateDatabase()) {
            var sql = new Sql($"SELECT * FROM {CrowdfundingConstants.Tables.CrowdfundingOfflineContributions.Name}").Append(whereClause);
            
            return await db.SingleOrDefaultAsync<CrowdfundingOfflineContributions>(sql);
        }
    }
    
    private async Task<(bool, CrowdfundingOfflineContributions)> GetCrowdfundingOfflineContributionsAsync(WebhookPledge pledge) {
        CrowdfundingOfflineContributions offlineContributions;
        
        if (pledge.Crowdfunding.FundraiserId.HasValue) {
            offlineContributions = await FindByFundraiserAsync(pledge.Crowdfunding.FundraiserId.GetValueOrThrow());
        } else {
            offlineContributions = await FindByCampaignAsync(pledge.Crowdfunding.CampaignId);
        }
        
        var isNew = !offlineContributions.HasValue();

        if (isNew) {
            offlineContributions = GetCrowdfundingOfflineContributionsModel(pledge);
        }

        return (isNew, offlineContributions);
    }

    private CrowdfundingOfflineContributions GetCrowdfundingOfflineContributionsModel(WebhookPledge pledge) {
        var crowdfundingOfflineContributions = new CrowdfundingOfflineContributions();

        crowdfundingOfflineContributions.PledgeId = pledge.Revision.Id;
        crowdfundingOfflineContributions.PledgeRevision = pledge.Revision.Number;
        crowdfundingOfflineContributions.CampaignId = pledge.Crowdfunding.CampaignId;
        crowdfundingOfflineContributions.FundraiserId = pledge.Crowdfunding.FundraiserId;
        crowdfundingOfflineContributions.TeamId = pledge.Crowdfunding.TeamId;
        crowdfundingOfflineContributions.Amount = pledge.BalanceSummary.OtherDonationsTotal.Amount;
        crowdfundingOfflineContributions.CurrencyCode = pledge.BalanceSummary.OtherDonationsTotal.Currency.Code;
        crowdfundingOfflineContributions.DonationsCount = pledge.BalanceSummary.OtherDonationsCount;

        return crowdfundingOfflineContributions;
    }
}