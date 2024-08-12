using N3O.Umbraco.Attributes;
using N3O.Umbraco.Content;
using N3O.Umbraco.Cropper.Models;
using N3O.Umbraco.Crowdfunding.Lookups;
using System.Collections.Generic;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Strings;

namespace N3O.Umbraco.Crowdfunding.Content;

[UmbracoContent(CrowdfundingConstants.Fundraiser.Alias)]
public class FundraiserContent : UmbracoContent<FundraiserContent> {
    public string Title => GetValue(x => x.Title);
    public string Description => GetValue(x => x.Description);
    public string Slug => GetValue(x => x.Slug);
    public HtmlEncodedString Body => GetValue(x => x.Body);
    public CroppedImage HeroImage => GetValue(x => x.HeroImage);
    public CampaignContent Campaign => GetPickedAs(x => x.Campaign);
    public IPublishedContent Owner => GetPickedAs(x => x.Owner);
    public TeamContent Team => GetPickedAs(x => x.Team);
    public IEnumerable<IPublishedContent> Tags => GetPickedAs(x => x.Tags);
    public FundraiserStatus Status => GetStaticLookupByNameAs(x => x.Status);
    public IEnumerable<CampaignImagesElement> CampaignImages => GetNestedAs(x => x.CampaignImages);
    public IEnumerable<FundraiserAllocationElement> Allocations => GetNestedAs(x => x.Allocations);
}