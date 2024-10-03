namespace N3O.Umbraco.Crowdfunding.Entities;

public class CrowdfunderRevision {
    /*
     * Key
     * Type (1 = Campaign, 2 = Fundraiser)
     * Id (Umbraco content ID)
     * Revision (umbraco content version)
     * Name
     * Url
     * Currency
     * GoalsTotalQuote
     * GoalsTotalBase
     * ActiveFrom
     * ActiveTo (nullable)
     *
     * When a campaign or fundraiser is published, one of the following happens:
     * Name change -> rename all rows with the content ID + rename in contributions
     * Goal total changes -> insert a new entry, and we need to close off the previous (set ActiveTo = today)
     * Activated -> Insert a new row into the table
     * Deactivated -> Update the last row in the table to set the ActiveTo column
     */    
}