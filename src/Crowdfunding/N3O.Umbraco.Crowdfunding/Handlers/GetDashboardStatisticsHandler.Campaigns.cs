using N3O.Umbraco.Attributes;
using N3O.Umbraco.Crm.Lookups;
using System.Linq;
using N3O.Umbraco.Crowdfunding.Criteria;
using N3O.Umbraco.Crowdfunding.Entities;
using N3O.Umbraco.Crowdfunding.Models;
using System.Threading.Tasks;
using N3O.Umbraco.Financial;
using NPoco;
using System;
using Umbraco.Cms.Infrastructure.Persistence;

namespace N3O.Umbraco.Crowdfunding.Handlers;

public partial class GetDashboardStatisticsHandler
{
    private async Task PopulateCampaignsAsync(IUmbracoDatabase db,
                                              DashboardStatisticsCriteria criteria,
                                              DashboardStatisticsRes res) {
        res.Campaigns = new CampaignStatisticsRes(); 
        
        await PopulateCrowdfunderAsync(db, criteria, res.Campaigns);
    }
    
}