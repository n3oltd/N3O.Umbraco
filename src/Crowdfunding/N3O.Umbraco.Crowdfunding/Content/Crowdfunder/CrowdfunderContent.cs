using N3O.Umbraco.Attributes;
using N3O.Umbraco.Content;
using N3O.Umbraco.Crm.Models;
using N3O.Umbraco.Cropper.Models;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Financial;
using System;
using System.Collections.Generic;
using Umbraco.Cms.Core.Strings;

namespace N3O.Umbraco.Crowdfunding.Content;

public abstract class CrowdfunderContent<T> : UmbracoContent<T>, ICrowdfunderContent, ICrowdfunder
    where T : CrowdfunderContent<T> {
    public Guid Key => Content().Key;
    public CroppedImage BackgroundImage => GetValue(x => x.BackgroundImage);
    public HtmlEncodedString Body => GetValue(x => x.Body);
    public Currency Currency => GetValue(x => x.Currency);
    public string Description => GetValue(x => x.Description);
    public IEnumerable<GoalElement> Goals => GetNestedAs(x => x.Goals);
    public IEnumerable<HeroImagesElement> HeroImages => GetNestedAs(x => x.HeroImages);
    [UmbracoProperty(CrowdfundingConstants.Crowdfunder.Properties.Name)]
    public string Name => GetValue(x => x.Name);
    
    public abstract Guid CampaignId { get; }
    public abstract string CampaignName { get; }
    public abstract Guid? TeamId { get; }
    public abstract string TeamName { get; }
    public abstract Guid? FundraiserId { get; }
    public abstract string FundraiserUrl { get; }
    
    public Guid Id => Key;
    public string Url => Content().AbsoluteUrl();
    IEnumerable<ICrowdfunderGoal> ICrowdfunder.Goals => Goals;
}