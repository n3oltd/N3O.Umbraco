using N3O.Umbraco.Financial;

namespace N3O.Umbraco.CrowdFunding.Models.FundraisingPage;

public class CrowdfundingPageCampaignFundraisers {
    public string Name { get; set; }
    public string AvatarLink { get; set; }
    public string Role { get; set; }
    public Money RaisedAmount { get; set; }
    public Money TargetAmount { get; set; }
}