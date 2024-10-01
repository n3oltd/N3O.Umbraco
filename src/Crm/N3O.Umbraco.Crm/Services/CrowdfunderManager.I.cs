using N3O.Umbraco.Crm.Models;
using System.Threading.Tasks;

namespace N3O.Umbraco.Crm;

public interface ICrowdfunderManager {
    Task CreateCampaignAsync(ICampaign campaign);
    Task CreateFundraiserAsync(IFundraiser fundraiser);
    Task UpdateCrowdfunderAsync(string id, ICrowdfunder crowdfunder, bool toggleStatus);
}