using N3O.Umbraco.Attributes;
using N3O.Umbraco.Cloud.Platforms.Extensions;
using N3O.Umbraco.Cloud.Platforms.Lookups;
using N3O.Umbraco.Content;
using N3O.Umbraco.Exceptions;
using N3O.Umbraco.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Strings;
using Umbraco.Community.Contentment.DataEditors;
using Umbraco.Extensions;

namespace N3O.Umbraco.Cloud.Platforms.Content;

[UmbracoContent(PlatformsConstants.Campaigns.CompositionAlias)]
public class CampaignContent : UmbracoContent<CampaignContent> {
    private static readonly string ScheduledGivingCampaignAlias = AliasHelper<ScheduledGivingCampaignContent>.ContentTypeAlias();
    private static readonly string StandardCampaignAlias = AliasHelper<StandardCampaignContent>.ContentTypeAlias();
    private static readonly string TelethonCampaignAlias = AliasHelper<TelethonCampaignContent>.ContentTypeAlias();
    
    public override void SetContent(IPublishedContent content) {
        base.SetContent(content);
        
        if (Type == CampaignTypes.Standard) {
            Standard = new StandardCampaignContent();
            Standard.SetContent(content);
        } else if (Type == CampaignTypes.Telethon) {
            Telethon = new TelethonCampaignContent();
            Telethon.SetContent(content);
        } else if (Type == CampaignTypes.ScheduledGiving) {
            ScheduledGiving = new ScheduledGivingCampaignContent();
            ScheduledGiving.SetContent(content);
        } else {
            throw UnrecognisedValueException.For(Type);
        }
    }

    public override void SetVariationContext(VariationContext variationContext) {
        base.SetVariationContext(variationContext);
        
        Standard?.SetVariationContext(variationContext);
        Telethon?.SetVariationContext(variationContext);
        ScheduledGiving?.SetVariationContext(variationContext);
    }

    public string Name => Content().Name;
    public Guid Key => Content().Key;
    
    public IHtmlEncodedString Description => GetValue(x => x.Description);
    public string Notes => GetValue(x => x.Notes);
    public IReadOnlyDictionary<string, string> Tags => GetConvertedValue<IEnumerable<DataListItem>, IReadOnlyDictionary<string, string>>(x => x.Tags, x => x.ToTagsDictionary());
    public MediaWithCrops Icon => GetValue(x => x.Icon);
    public MediaWithCrops Image => GetValue(x => x.Image);
    public decimal Target => GetValue(x => x.Target);
    
    public string DonationFormEmbedCode => GetValue(x => x.DonationFormEmbedCode);
    public string DonationButtonEmbedCode => GetValue(x => x.DonationButtonEmbedCode);

    public IEnumerable<OfferingContent> Offerings => Content().Descendants()
                                                              .Where(x => x.IsComposedOf(AliasHelper<OfferingContent>.ContentTypeAlias()))
                                                              .As<OfferingContent>();

    public OfferingContent DefaultOffering => Offerings.FirstOrDefault();
    
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