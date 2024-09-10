﻿using N3O.Umbraco.Attributes;
using N3O.Umbraco.Content;
using N3O.Umbraco.Cropper.Models;
using N3O.Umbraco.Crowdfunding.Lookups;
using N3O.Umbraco.Financial;
using N3O.Umbraco.Lookups;
using System.Collections.Generic;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Strings;

namespace N3O.Umbraco.Crowdfunding.Content;

[UmbracoContent(CrowdfundingConstants.Fundraiser.Alias)]
public class FundraiserContent : UmbracoContent<FundraiserContent> {
    public string Title => GetValue(x => x.Title);
    public string Description => GetValue(x => x.Description);
    public string Currency => GetValue(x => x.Currency);
    public string AccountReference => GetValue(x => x.AccountReference);
    public string AllocationsHash => GetValue(x => x.AllocationsHash);
    public string Slug => GetValue(x => x.Slug);
    public HtmlEncodedString Body => GetValue(x => x.Body);
    public CroppedImage BackgroundImage => GetValue(x => x.BackgroundImage);
    public CampaignContent Campaign => GetPickedAs(x => x.Campaign);
    public IPublishedContent Owner => GetPickedAs(x => x.Owner);
    public TeamContent Team => GetPickedAs(x => x.Team);
    public FundraiserStatus Status => GetStaticLookupByNameAs(x => x.Status);
    public IEnumerable<FundraiserGoalElement> Goals => GetNestedAs(x => x.Goals);
    public IEnumerable<HeroImagesElement> HeroImages => GetNestedAs(x => x.HeroImages);
    public IEnumerable<IPublishedContent> Tags => GetPickedAs(x => x.Tags);

    public Currency GetCurrency(ILookups lookups) {
        return lookups.FindById<Currency>(Currency);
    }
}