using N3O.Umbraco.Cloud.Platforms.Lookups;
using N3O.Umbraco.Content;
using N3O.Umbraco.Exceptions;
using N3O.Umbraco.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Community.Contentment.DataEditors;
using Umbraco.Extensions;

namespace N3O.Umbraco.Cloud.Platforms.Content;

public class CampaignContent : UmbracoContent<CampaignContent> {
    private static readonly string ScheduledGivingCampaignAlias = AliasHelper<ScheduledGivingCampaignContent>.ContentTypeAlias();
    private static readonly string StandardCampaignAlias = AliasHelper<StandardCampaignContent>.ContentTypeAlias();
    private static readonly string TelethonCampaignAlias = AliasHelper<TelethonCampaignContent>.ContentTypeAlias();
    
    public override void Content(IPublishedContent content) {
        base.Content(content);
        
        if (Type == CampaignTypes.Standard) {
            Standard = new StandardCampaignContent();
            Standard.Content(content);
        } else if (Type == CampaignTypes.Telethon) {
            Telethon = new TelethonCampaignContent();
            Telethon.Content(content);
        } else if (Type == CampaignTypes.ScheduledGiving) {
            ScheduledGiving = new ScheduledGivingCampaignContent();
            ScheduledGiving.Content(content);
        } else {
            throw UnrecognisedValueException.For(Type);
        }
    }
    
    public string Name => Content().Name;
    public Guid Key => Content().Key;
    
    public IEnumerable<DataListItem> AnalyticsTags => GetValue(x => x.AnalyticsTags);
    public MediaWithCrops Icon => GetValue(x => x.Icon);
    public MediaWithCrops Image => GetValue(x => x.Image);

    public IEnumerable<DesignationContent> Designations => Content()
                                                           .Descendants()
                                                           .Where(x => x.IsComposedOf(AliasHelper<DesignationContent>.ContentTypeAlias()))
                                                           .As<DesignationContent>();
    
    public ScheduledGivingCampaignContent ScheduledGiving { get; private set; }
    public StandardCampaignContent Standard { get; private set; }
    public TelethonCampaignContent Telethon { get; private set; }
    
    public CampaignType Type {
        get {
            if (Content().ContentType.Alias.EqualsInvariant(StandardCampaignAlias)) {
                return CampaignTypes.Standard;
            } else if (Content().ContentType.Alias.EqualsInvariant(TelethonCampaignAlias)) {
                return CampaignTypes.Telethon;
            } else if (Content().ContentType.Alias.EqualsInvariant(ScheduledGivingCampaignAlias)) {
                return CampaignTypes.ScheduledGiving;
            } else {
                throw UnrecognisedValueException.For(Content().ContentType.Alias);
            }
        }
    }
}