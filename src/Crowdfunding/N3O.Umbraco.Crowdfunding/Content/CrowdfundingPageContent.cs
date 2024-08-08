using N3O.Umbraco.Content;
using N3O.Umbraco.Cropper.Models;
using N3O.Umbraco.Crowdfunding.Lookups;
using System.Collections.Generic;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Strings;

namespace N3O.Umbraco.Crowdfunding.Content;

public class CrowdfundingPageContent : UmbracoContent<CrowdfundingPageContent> {
    public string FundraiserName => GetValue(x => x.FundraiserName);
    public string PageTitle => GetValue(x => x.PageTitle);
    public string PageDescription => GetValue(x => x.PageDescription);
    public string PageSlug => GetValue(x => x.PageSlug);
    public HtmlEncodedString AboutPage => GetValue(x => x.AboutPage);
    public CroppedImage HeroImageBanner => GetValue(x => x.HeroImageBanner);
    public CrowdfundingCampaignContent Campaign => GetPickedAs(x => x.Campaign);
    public CrowdfundingTeamContent Team => GetPickedAs(x => x.Team);
    public IEnumerable<IPublishedContent> PageTags => GetPickedAs(x => x.PageTags);
    public CrowdfundingPageStatus PageStatus => GetStaticLookupByNameAs(x => x.PageStatus);
    public IEnumerable<CrowdfundingPageCampaignImagesElement> CampaignImages => GetNestedAs(x => x.CampaignImages);
    public IEnumerable<CrowdfundingPageAllocationElement> Allocations => GetNestedAs(x => x.Allocations);
}