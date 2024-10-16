using Microsoft.Extensions.DependencyInjection;
using N3O.Umbraco.Attributes;
using N3O.Umbraco.Content;
using N3O.Umbraco.Crm.Lookups;
using N3O.Umbraco.Crm.Models;
using N3O.Umbraco.Cropper.Models;
using N3O.Umbraco.Crowdfunding.Extensions;
using N3O.Umbraco.Financial;
using System;
using System.Collections.Generic;
using System.Text;
using Umbraco.Cms.Core.Strings;

namespace N3O.Umbraco.Crowdfunding.Content;

public abstract class CrowdfunderContent<T> : UmbracoContent<T>, ICrowdfunderContent, ICrowdfunder
    where T : CrowdfunderContent<T> {
    public Guid Key => Content().Key;
    public CrowdfunderType Type => Content().ContentType.Alias.ToCrowdfunderType();
    public CroppedImage BackgroundImage => GetValue(x => x.BackgroundImage);
    public HtmlEncodedString Body => GetValue(x => x.Body);
    public Currency Currency => GetValue(x => x.Currency);
    public string Description => GetValue(x => x.Description);
    public IEnumerable<GoalElement> Goals => GetNestedAs(x => x.Goals);
    public IEnumerable<HeroImagesElement> HeroImages => GetNestedAs(x => x.HeroImages);
    public IEnumerable<TagContent> Tags => GetPickedAs(x => x.Tags);

    [UmbracoProperty(CrowdfundingConstants.Crowdfunder.Properties.Name)]
    public string Name => GetValue(x => x.Name);
    public string OpenGraphImagePath => GetValue(x => x.OpenGraphImagePath);
    public DateTime CreatedDated => Content().CreateDate;

    public bool ToggleStatus => GetValue(x => x.ToggleStatus);
    public CrowdfunderStatus Status => GetStaticLookupByNameAs(x => x.Status);

    public abstract Guid CampaignId { get; }
    public abstract string CampaignName { get; }
    public abstract Guid? TeamId { get; }
    public abstract string TeamName { get; }
    public abstract Guid? FundraiserId { get; }
    
    public Guid Id => Key;
    IEnumerable<ICrowdfunderGoal> ICrowdfunder.Goals => Goals;
    
    public string GetFullText() {
        var sb = new StringBuilder();
        sb.AppendJoin(' ' , Name);
        sb.AppendJoin(' ' , CampaignName);
        sb.AppendJoin(' ', Description);
        sb.AppendJoin(' ', Type.Name);

        PopulateFullText(sb);

        return sb.ToString();
    }
    
    public string Url(IServiceProvider serviceProvider) {
        return Url(serviceProvider.GetRequiredService<ICrowdfundingUrlBuilder>());
    }
    
    public abstract string Url(ICrowdfundingUrlBuilder urlBuilder);
    
    protected abstract void PopulateFullText(StringBuilder sb);
}