using N3O.Umbraco.Crm.Models;
using System.Threading.Tasks;

namespace N3O.Umbraco.Crm;

public interface ICrowdfunderManager {
    Task CreateCampaignAsync(ICampaign campaign, string webhookUrl);
    Task CreateFundraiserAsync(IFundraiser fundraiser, string webhookUrl);
    Task UpdateCrowdfunderAsync(string id, ICrowdfunder crowdfunder, bool toggleStatus, string webhookUrl);
}