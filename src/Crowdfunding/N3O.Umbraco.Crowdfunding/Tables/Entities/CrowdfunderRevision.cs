using NPoco;
using Umbraco.Cms.Infrastructure.Persistence.DatabaseAnnotations;
using static N3O.Umbraco.Crowdfunding.CrowdfundingConstants;

namespace N3O.Umbraco.Crowdfunding.Entities;

[TableName(Tables.CrowdfunderRevisions.Name)]
[PrimaryKey("Id")]
public class CrowdfunderRevision {
    [PrimaryKeyColumn(Name = Tables.CrowdfunderRevisions.PrimaryKey)]
    public int Id { get; set; }
}

    /*
     * Type (1 = Campaign, 2 = Fundraiser)
     * ContentKey (Umbraco content ID)
     * ContentRevision (umbraco content version)
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