using N3O.Umbraco.Financial;

namespace N3O.Umbraco.Crowdfunding.Models;

/*
 * We can sometimes combine multiple queries into one rather than executing
 * multiple. If we do we execute multiple we should use Tasks to run them
 * in parallel and await the combined results.
 *
 * Use Redgate SQL data generator to try inserting millions of rows into a DB
 * to check how your queries scale.
 */
public class DashboardStatisticsRes {
    public CurrencyRes BaseCurrency { get; set; }
    public ContributionStatisticsRes Contributions { get; set; }
    public AllocationStatisticsRes Allocations { get; set; }
    // TODO Put the same models but for fundraisers
    public CampaignStatisticsRes Campaigns { get; set; }
}

/*
 * SELECT query from Online and Offline contributions. We will need to refactor offline contributions
 * so we cna apply date ranges. We will do a simple aggregate query with BETWEEN date clause.
 */
public class ContributionStatisticsRes {
    public MoneyRes Total { get; set; }
    public MoneyRes Average { get; set; }
    public int Count { get; set; }
}

/*
 * For this we will need to store allocation summary directly in the table for
 * online and offline so we can do a BETWEEN where clause with a GROUP BY
 * on the allocation summary. Remember we do not want all the results so the query
 * should specify the top 10.
 */
public class AllocationStatisticsRes {
    public AllocationStatisticsItemRes TopItems { get; set; }
}

public class AllocationStatisticsItemRes {
    public string Summary { get; set; }
    public MoneyRes Total { get; set; }
}


public class CampaignStatisticsRes {
    /*
     * For campaigns this can just come from the total contributions that
     * we will already have, but for fundraisers we will need to recalculate this
     * by filtering the sum(online or offline contrubitions) where fundraiserId is not null
     */
    public MoneyRes ContributionsTotal { get; set; }
    
    /*
     * We can do a BETWEEN where clause with a GROUP BY on the campaign name.
     * Remember we do not want all the results so the query should specify the top 10.
     */
    public CampaignStatisticsItemRes TopItems { get; set; }
    
    /*
     * SELECT COUNT(*), SUM(BaseAmount) FROM CrowdfunderStates WHERE ActiveFrom >= {FromDate} AND (ActiveTo <= {ToDate} OR ActiveTo IS NULL)
     * AND Type = 1
     */
    public int ActiveCount { get; set; }
    public MoneyRes GoalsTotal { get; set; }
}

public class CampaignStatisticsItemRes {
    public string Name { get; set; }
    public MoneyRes Total { get; set; }
}

/*
 * CrowdfunderStates (not a good name) table in SQL with the following columns
 * Key
 * Type (1 = Campaign, 2 = Fundraiser)
 * Id (Umbraco content ID)
 * Name
 * Currency
 * GoalsTotalQuote
 * GoalsTotalBase
 * ActiveFrom
 * ActiveTo (nullable)
 *
 * When a campaign or fundraiser is published, one of the following happens:
 * Name change -> rename all rows with the content ID
 * Goal total changes -> insert a new entry, and we need to close off the previous (set ActiveTo = today)
 * Activated -> Insert a new row into the table
 * Deactivated -> Update the last row in the table to set the ActiveTo column
*/