using N3O.Umbraco.Attributes;
using N3O.Umbraco.Content;
using N3O.Umbraco.Giving.Lookups;

namespace N3O.Umbraco.Crowdfunding.Content;

[UmbracoContent(CrowdfundingConstants.CampaignGoal.Fund.Alias)]
public class FundCampaignGoalElement : UmbracoElement<FundCampaignGoalElement> {
    public DonationItem DonationItem => GetAs(x => x.DonationItem);
}
