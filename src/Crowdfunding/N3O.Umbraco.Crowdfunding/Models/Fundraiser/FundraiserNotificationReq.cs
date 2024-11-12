using N3O.Umbraco.Attributes;
using N3O.Umbraco.Crowdfunding.Lookups;

namespace N3O.Umbraco.Crowdfunding.Models;

public class FundraiserNotificationReq {
    [Name("Type")]
    public FundraiserNotificationType Type { get; set; }
    
    [Name("Fundraiser")]
    public FundraiserNotificationViewModel Fundraiser { get; set; }
    
    [Name("Goal")]
    public FundraiserGoalsTotalViewModel Goal { get; set; }
}