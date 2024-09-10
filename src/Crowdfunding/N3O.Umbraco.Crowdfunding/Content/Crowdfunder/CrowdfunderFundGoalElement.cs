using N3O.Umbraco.Content;
using N3O.Umbraco.Giving.Lookups;

namespace N3O.Umbraco.Crowdfunding.Content;

public class CrowdfunderFundGoalElement : UmbracoElement<CrowdfunderFundGoalElement> {
    public DonationItem DonationItem => GetAs(x => x.DonationItem);
}