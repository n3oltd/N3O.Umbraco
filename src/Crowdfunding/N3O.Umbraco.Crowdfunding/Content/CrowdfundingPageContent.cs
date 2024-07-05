using N3O.Umbraco.Content;
using N3O.Umbraco.Giving.Crowdfunding.Lookups;

namespace N3O.Umbraco.Crowdfunding.Content;

public class CrowdfundingPageContent : UmbracoContent<CrowdfundingPageContent> {
    public CrowdfundingCampaignContent Campaign => GetPickedAs(x => x.Campaign);
    public CrowdfundingPageStatus PageStatus => GetStaticLookupByNameAs(x => x.PageStatus);
    public CrowdfundingTeamContent Team => GetPickedAs(x => x.Team);
    public string PageTitle => GetValue(x => x.PageTitle);
    public string PageSlug => GetValue(x => x.PageSlug);
    public string FundraiserName => GetValue(x => x.FundraiserName);
}