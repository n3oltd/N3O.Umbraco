using N3O.Umbraco.Attributes;
using N3O.Umbraco.Content;
using N3O.Umbraco.Giving.Allocations.Lookups;

namespace N3O.Umbraco.Crowdfunding.Content;

[UmbracoContent(CrowdfundingConstants.CampaignGoalOption.Fund.Alias)]
public class CampaignFundGoalOptionElement : UmbracoElement<CampaignFundGoalOptionElement> {
    public DonationItem DonationItem => GetValue(x => x.DonationItem);
}