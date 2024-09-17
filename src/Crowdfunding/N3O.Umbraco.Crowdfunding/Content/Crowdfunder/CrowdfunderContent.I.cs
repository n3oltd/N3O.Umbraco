using N3O.Umbraco.Content;
using N3O.Umbraco.Cropper.Models;
using N3O.Umbraco.Financial;
using System;
using System.Collections.Generic;
using Umbraco.Cms.Core.Strings;

namespace N3O.Umbraco.Crowdfunding.Content;

public interface ICrowdfunderContent {
    public Guid Key { get; }
    public CroppedImage BackgroundImage { get; }
    public HtmlEncodedString Body { get; }
    public Currency Currency { get; }
    public string Description { get; }
    public IEnumerable<GoalElement> Goals { get; }
    public IEnumerable<HeroImagesElement> HeroImages { get; }
    public string Name { get; }
    
    Guid CampaignId { get; }
    string CampaignName { get; }
    Guid? TeamId { get; }
    string TeamName { get; }
    Guid? FundraiserId { get; }

    string Url(IContentLocator contentLocator);
}