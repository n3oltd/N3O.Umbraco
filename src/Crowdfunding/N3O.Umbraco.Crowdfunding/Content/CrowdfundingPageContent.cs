using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Giving.Crowdfunding.Lookups;
using N3O.Umbraco.Lookups;
using System.Linq;
using Umbraco.Extensions;
using static N3O.Umbraco.Crowdfunding.CrowdfundingConstants;

namespace N3O.Umbraco.Crowdfunding.Content;

public class CrowdfundingPageContent : UmbracoContent<CrowdfundingPageContent> {
    public CrowdfundingCampaignContent Campaign => GetPickedAs(x => x.Campaign);
    public CrowdfundingPageStatus PageStatus => GetPageStatus();
    public CrowdfundingTeamContent Team => GetPickedAs(x => x.Team);
    public string PageTitle => GetPickedAs(x => x.PageTitle);

    private CrowdfundingPageStatus GetPageStatus() {
        var status = Content().Value<string>(CrowdfundingPage.Properties.PageStatus);
        
        return StaticLookups.GetAll<CrowdfundingPageStatus>().SingleOrDefault(x => x.Name.EqualsInvariant(status));
    } 
}