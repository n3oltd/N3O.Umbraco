using N3O.Umbraco.Attributes;
using CrowdfunderType = N3O.Umbraco.Crm.Lookups.CrowdfunderType;

namespace N3O.Umbraco.Crowdfunding.Criteria;

public class FundraiserPagesCriteria {
    [Name("Current Page")]
    public int? CurrentPage { get; set; }
    
    [Name("Page Size")]
    public int? PageSize { get; set; }
}