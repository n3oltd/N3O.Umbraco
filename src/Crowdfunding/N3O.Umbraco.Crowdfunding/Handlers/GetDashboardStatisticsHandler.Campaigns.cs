using N3O.Umbraco.Crm.Lookups;
using N3O.Umbraco.Crowdfunding.Criteria;
using N3O.Umbraco.Crowdfunding.Models;
using System.Threading.Tasks;
using Umbraco.Cms.Infrastructure.Persistence;

namespace N3O.Umbraco.Crowdfunding.Handlers;

public partial class GetDashboardStatisticsHandler {
    private async Task PopulateCampaignsAsync(IUmbracoDatabase db,
                                              DashboardStatisticsCriteria criteria,
                                              DashboardStatisticsRes res) {
        res.Campaigns = new CampaignStatisticsRes(); 
        
        await PopulateCrowdfunderAsync(db, CrowdfunderTypes.Campaign, criteria, res.Campaigns);
    }
}