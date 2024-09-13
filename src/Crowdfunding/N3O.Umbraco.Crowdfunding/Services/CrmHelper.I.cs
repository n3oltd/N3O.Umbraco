using N3O.Umbraco.Crm.Engage.Clients;
using N3O.Umbraco.Crowdfunding.Content;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Currency = N3O.Umbraco.Financial.Currency;

namespace N3O.Umbraco.CrowdFunding;

public interface ICrmHelper {
    Task<CampaignRes> CreatePledgeForCampaignAsync(CampaignContent campaignContent);
    Task<FundraiserRes> CreatePledgeForFundraiserAsync(FundraiserContent fundraiserContent);
    Task UpdateAllocationsAsync(Guid id, Currency currency, IEnumerable<GoalElement> goals);
    Task UpdateNameAsync(Guid id, string name);
    Task UpdateSlugAsync(Guid id, string url);
}