using N3O.Umbraco.Content;
using N3O.Umbraco.Giving.Lookups;

namespace N3O.Umbraco.Crowdfunding.Content;

public abstract class CrowdfundingFundGoalElement : UmbracoElement<CrowdfundingFundGoalElement> {
    public DonationItem DonationItem => GetAs(x => x.DonationItem);
}