using N3O.Umbraco.Attributes;
using N3O.Umbraco.Content;
using N3O.Umbraco.Cloud.Engage.Models;
using N3O.Umbraco.Giving.Allocations.Lookups;

namespace N3O.Umbraco.Crowdfunding.Content;

[UmbracoContent(CrowdfundingConstants.Goal.Fund.Alias)]
public class FundGoalElement : UmbracoElement<FundGoalElement>, IFundCrowdfunderGoal {
    public DonationItem DonationItem => GetValue(x => x.DonationItem);
}