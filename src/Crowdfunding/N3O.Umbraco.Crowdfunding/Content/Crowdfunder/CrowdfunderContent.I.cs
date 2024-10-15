using N3O.Umbraco.Crm.Lookups;
using N3O.Umbraco.Cropper.Models;
using N3O.Umbraco.Financial;
using System;
using System.Collections.Generic;
using Umbraco.Cms.Core.Strings;

namespace N3O.Umbraco.Crowdfunding.Content;

public interface ICrowdfunderContent {
    public Guid Key { get; }
    public CrowdfunderType Type { get; }
    public CrowdfunderStatus Status { get; }
    public CroppedImage BackgroundImage { get; }
    public HtmlEncodedString Body { get; }
    public Currency Currency { get; }
    public string Description { get; }
    public IEnumerable<GoalElement> Goals { get; }
    public IEnumerable<HeroImagesElement> HeroImages { get; }
    public IEnumerable<TagContent> Tags { get; }
    public string Name { get; }
    public string OpenGraphImagePath { get; }
    
    Guid CampaignId { get; }
    string CampaignName { get; }
    Guid? TeamId { get; }
    string TeamName { get; }
    Guid? FundraiserId { get; }

    string GetFullText();
    string Url(ICrowdfundingUrlBuilder urlBuilder);
}