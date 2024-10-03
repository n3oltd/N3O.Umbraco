using N3O.Umbraco.Crowdfunding.Criteria;
using N3O.Umbraco.Crowdfunding.Models;
using System.Threading.Tasks;
using Umbraco.Cms.Infrastructure.Persistence;

namespace N3O.Umbraco.Crowdfunding.Handlers;

public partial class GetDashboardStatisticsHandler {
    private async Task PopulateCampaignsAsync(IUmbracoDatabase db,
                                              DashboardStatisticsCriteria criteria,
                                              DashboardStatisticsRes res) {
        /*
SQL Query
         */
        
        res.Campaigns = new CampaignStatisticsRes();
        
        await PopulateCrowdfunderAsync(db, criteria, res.Campaigns);
    }
}