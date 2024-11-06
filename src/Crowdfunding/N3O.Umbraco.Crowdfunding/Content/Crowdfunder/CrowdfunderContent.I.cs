using N3O.Umbraco.Crm.Lookups;
using N3O.Umbraco.Cropper.Models;
using N3O.Umbraco.Financial;
using System;
using System.Collections.Generic;
using Umbraco.Cms.Core.Strings;

namespace N3O.Umbraco.Crowdfunding.Content;

public interface ICrowdfunderContent {
    Guid Key { get; }
    CrowdfunderType Type { get; }
    CrowdfunderStatus Status { get; }
    CroppedImage BackgroundImage { get; }
    HtmlEncodedString Body { get; }
    Currency Currency { get; }
    string Description { get; }
    IEnumerable<GoalElement> Goals { get; }
    IEnumerable<HeroImagesElement> HeroImages { get; }
    IEnumerable<TagContent> Tags { get; }
    string Name { get; }
    string OpenGraphImagePath { get; }
    DateTime CreatedDate { get; }
    
    Guid CampaignId { get; }
    string CampaignName { get; }
    Guid? TeamId { get; }
    string TeamName { get; }
    Guid? FundraiserId { get; }
    string FundraiserName { get; }

    string GetFullText();
    string Url(ICrowdfundingUrlBuilder urlBuilder);
}