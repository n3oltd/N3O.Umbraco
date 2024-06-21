using N3O.Umbraco.Content;

namespace N3O.Umbraco.Crowdfunding.Content;

public class CrowdfundingPageContent : UmbracoContent<CrowdfundingPageContent> {
    public CrowdfundingCampaignContent Campaign => GetPickedAs(x => x.Campaign);
    public bool SetupComplete => GetValue(x => x.SetupComplete);
    public CrowdfundingTeamContent Team => GetPickedAs(x => x.Team);
}