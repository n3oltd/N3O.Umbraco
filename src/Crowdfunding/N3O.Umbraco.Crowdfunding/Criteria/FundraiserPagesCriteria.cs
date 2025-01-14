using N3O.Umbraco.Attributes;

namespace N3O.Umbraco.Crowdfunding.Criteria;

public class FundraiserPagesCriteria {
    [Name("Current Page")]
    public int? CurrentPage { get; set; }
    
    [Name("Page Size")]
    public int? PageSize { get; set; }
}