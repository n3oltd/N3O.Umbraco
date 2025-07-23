using N3O.Umbraco.Attributes;
using N3O.Umbraco.Content;
using N3O.Umbraco.Giving.Allocations.Lookups;
using N3O.Umbraco.Lookups;

namespace N3O.Umbraco.Crowdfunding.Content;

[UmbracoContent(CrowdfundingConstants.CampaignGoalOption.Fund.Alias)]
public class CampaignFundGoalOptionElement : UmbracoElement<CampaignFundGoalOptionElement> {
    public DonationItem GetDonationItem(ILookups lookups) {
        return GetLookup<DonationItem>(lookups, CrowdfundingConstants.CampaignGoalOption.Fund.Properties.DonationItem);
    }
}