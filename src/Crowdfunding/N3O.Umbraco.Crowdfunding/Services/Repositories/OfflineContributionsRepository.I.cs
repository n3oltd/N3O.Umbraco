using N3O.Umbraco.Crowdfunding.Entities;
using N3O.Umbraco.Crowdfunding.Models;
using System;
using System.Threading.Tasks;

namespace N3O.Umbraco.CrowdFunding.Services;

public interface  IOfflineContributionsRepository {
    Task AddOrUpdateAsync(WebhookPledge pledge);
    Task<CrowdfundingOfflineContributions> FindByCampaignAsync(Guid campaignId);
    Task<CrowdfundingOfflineContributions> FindByFundraiserAsync(Guid fundraiserId);
}