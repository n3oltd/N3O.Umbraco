﻿using N3O.Umbraco.Attributes;
using N3O.Umbraco.Content;
using N3O.Umbraco.Cropper.Models;
using System.Collections.Generic;
using Umbraco.Cms.Core.Strings;

namespace N3O.Umbraco.Crowdfunding.Content;

[UmbracoContent(CrowdfundingConstants.Campaign.Alias)]
public class CampaignContent : UmbracoContent<CampaignContent> {
    public string Title => GetValue(x => x.Title);
    public string Description => GetValue(x => x.Description);
    public HtmlEncodedString Body => GetValue(x => x.Body);
    public CroppedImage BackgroundImage => GetValue(x => x.BackgroundImage);
    public IEnumerable<HeroImagesElement> HeroImages => GetNestedAs(x => x.HeroImages);
    public IEnumerable<CampaignGoalElement> Goals => GetNestedAs(x => x.Goals);
}