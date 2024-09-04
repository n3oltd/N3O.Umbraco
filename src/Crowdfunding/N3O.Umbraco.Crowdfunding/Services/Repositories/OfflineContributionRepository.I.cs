using N3O.Umbraco.Crowdfunding.Models;
using N3O.Umbraco.Financial;
using System;
using System.Threading.Tasks;

namespace N3O.Umbraco.CrowdFunding.Services;

public interface IOfflineContributionRepository {
    Task AddOrUpdateAsync(ICrowdfunderInfo crowdfunder, Money total, int count);
    Task<(Money Total, int Count)> SumByCampaignIdAsync(Guid campaignId);
    Task<(Money Total, int Count)> SumByFundraiserIdAsync(Guid fundraiserId);
}