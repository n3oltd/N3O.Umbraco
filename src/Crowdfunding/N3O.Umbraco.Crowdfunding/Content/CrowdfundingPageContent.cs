using N3O.Umbraco.Content;
using N3O.Umbraco.Crowdfunding.Lookups;

namespace N3O.Umbraco.Crowdfunding.Content;

public class CrowdfundingPageContent : UmbracoContent<CrowdfundingPageContent> {
    public CrowdfundingCampaignContent Campaign => GetPickedAs(x => x.Campaign);
    public string FundraiserName => GetValue(x => x.FundraiserName);
    public string PageSlug => GetValue(x => x.PageSlug);
    public CrowdfundingPageStatus PageStatus => GetStaticLookupByNameAs(x => x.PageStatus);
    public string PageTitle => GetValue(x => x.PageTitle);
    public CrowdfundingTeamContent Team => GetPickedAs(x => x.Team);
}