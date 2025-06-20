using N3O.Umbraco.Cloud.Engage.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace N3O.Umbraco.Cloud.Engage;

public interface ICrowdfunderManager {
    Task CreateCampaignAsync(ICampaign campaign, IEnumerable<string> webhookUrls);
    Task CreateFundraiserAsync(IFundraiser fundraiser, IEnumerable<string> webhookUrls);
    Task UpdateCrowdfunderAsync(string id, ICrowdfunder crowdfunder, bool toggleStatus, IEnumerable<string> webhookUrls);
}