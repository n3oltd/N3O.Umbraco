using N3O.Umbraco.Crowdfunding.Criteria;
using N3O.Umbraco.Crowdfunding.Models;
using System.Threading.Tasks;
using Umbraco.Cms.Infrastructure.Persistence;

namespace N3O.Umbraco.Crowdfunding.Handlers;

public partial class GetDashboardStatisticsHandler {
    private async Task PopulateCrowdfunderAsync(IUmbracoDatabase db,
                                                DashboardStatisticsCriteria criteria,
                                                CrowdfunderStatisticsRes res) {
                    /*
            For crowdfunders (fundraisers + campaigns):
            
            We should first select total, count from the contributions table and group this by crowdfunder ID (as fundraiser's can have the same name), e.g.
            
            Type		Campaign Name	Fundraiser Name		Url	Total		Count
            Campaign	Campaign 1					            £100		50
            Campaign	Campaign 2					            £150		100
            Fundraiser	Campaign 2	    Fundraiser 1			£150		100
            Fundraiser	Campaign 1	    Fundraiser 2			£250		200
            
            Once we have these we can split them into campaigns and fundraisers. The above table gives us the data we need for:
            
            Number of {Campaigns|Fundraisers}
            Fundraisers by Campaign
            Total Raised per Fundraiser
            
            
            From this table we can get the top 10 campaign or fundraiser IDs
            
            SELECT * FROM CrowdfunderRevisions WHERE Id IN (... list of at most 20 IDs...)
            
            As this is a table of revisions, we can potentially get multiple rows for the same crowdfunder. For the goal total,
            we want to use the most recent revision that is in the date period specified by the user, e.g. if date range is
            
            2024-09-01 to 2024-09-30
            
            CrowdfunderRevisions
            FundraiserId	Revision	ActiveFrom		ActiveTo	GoalTotal
            1	            	1		2024-09-01		2024-09-15	    100
            1	            	2		2024-09-15		2024-10-01	    125
            1	               	3		2024-10-01				        100
            
            In this case, we would select revision 2, i.e. £125 as that is the highest revision that satisfies the date criteria
                     */
                    
                    // Alias columns, group by date and use SQL parameters in SDK for the start and end dates
                    //var topItems = await db.FetchAsync<FundraiserStatisticsItemRes>($"SELECT * FROM");
                    
                    
                    
                    
        /*
         * 
         */

        res.Count = 0;
        
    }
}

