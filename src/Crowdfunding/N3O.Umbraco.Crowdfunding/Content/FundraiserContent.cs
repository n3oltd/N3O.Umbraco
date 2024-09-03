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
    public string AccountReference => GetValue(x => x.AccountReference);
    public string AllocationsHash => GetValue(x => x.AllocationsHash);
    public string PledgeRevisionId => GetValue(x => x.PledgeRevisionId);
    public string Slug => GetValue(x => x.Slug);
    public HtmlEncodedString Body => GetValue(x => x.Body);
    public CroppedImage BackgroundImage => GetValue(x => x.BackgroundImage);
    public CampaignContent Campaign => GetPickedAs(x => x.Campaign);
    public IPublishedContent Owner => GetPickedAs(x => x.Owner);
    public TeamContent Team => GetPickedAs(x => x.Team);
    public FundraiserStatus Status => GetStaticLookupByNameAs(x => x.Status);
    public IEnumerable<HeroImagesElement> HeroImages => GetNestedAs(x => x.HeroImages);
    public IEnumerable<FundraiserGoalElement> Allocations => GetNestedAs(x => x.Allocations);
}