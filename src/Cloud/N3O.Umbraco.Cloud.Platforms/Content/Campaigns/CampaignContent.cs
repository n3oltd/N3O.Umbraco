using N3O.Umbraco.Attributes;
using N3O.Umbraco.Cloud.Lookups;
using N3O.Umbraco.Cloud.Platforms.Clients;
using N3O.Umbraco.Cloud.Platforms.Lookups;
using N3O.Umbraco.Content;
using N3O.Umbraco.Exceptions;
using N3O.Umbraco.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Extensions;
using CampaignType = N3O.Umbraco.Cloud.Platforms.Lookups.CampaignType;

namespace N3O.Umbraco.Cloud.Platforms.Content;

[UmbracoContent(PlatformsConstants.Campaigns.CompositionAlias)]
public class CampaignContent : UmbracoContent<CampaignContent> {
    private static readonly string QurbaniCampaignAlias = AliasHelper<QurbaniCampaignContent>.ContentTypeAlias();
    private static readonly string ScheduledGivingCampaignAlias = AliasHelper<ScheduledGivingCampaignContent>.ContentTypeAlias();
    private static readonly string StandardCampaignAlias = AliasHelper<StandardCampaignContent>.ContentTypeAlias();
    private static readonly string TelethonCampaignAlias = AliasHelper<TelethonCampaignContent>.ContentTypeAlias();
    
    public override void SetContent(IPublishedContent content) {
        base.SetContent(content);

        FormContent = new DonationFormContentContent();
        FormContent.SetContent(content);

        if (Type == CampaignTypes.Standard) {
            Standard = new StandardCampaignContent();
            Standard.SetContent(content);
        } else if (Type == CampaignTypes.Qurbani) {
            Qurbani = new QurbaniCampaignContent();
            Qurbani.SetContent(content);
        } else if (Type == CampaignTypes.ScheduledGiving) {
            ScheduledGiving = new ScheduledGivingCampaignContent();
            ScheduledGiving.SetContent(content);
        } else if (Type == CampaignTypes.Telethon) {
            Telethon = new TelethonCampaignContent();
            Telethon.SetContent(content);
        } else {
            throw UnrecognisedValueException.For(Type);
        }
    }
    
    public void PopulateContributionInfo(ICdnClient cdnClient, PlatformsContributionInfoReq platformsContribution) {
        var publishedCampaign = cdnClient.DownloadPublishedContentAsync<PublishedCampaign>(PublishedFileKinds.Campaign,
                                                                                           $"{Key}.json",
                                                                                           JsonSerializers.Simple)
                                         .GetAwaiter().GetResult();
        
        platformsContribution.Campaign = new CampaignInfoReq();
        platformsContribution.Campaign.Id = publishedCampaign.Id;
        platformsContribution.Campaign.Reference = publishedCampaign.Reference;
    }

    public override void SetVariationContext(VariationContext variationContext) {
        base.SetVariationContext(variationContext);

        FormContent?.SetVariationContext(variationContext);
        Qurbani?.SetVariationContext(variationContext);
        Standard?.SetVariationContext(variationContext);
        Telethon?.SetVariationContext(variationContext);
        ScheduledGiving?.SetVariationContext(variationContext);
    }

    public string Name => Content().Name;
    public Guid Key => Content().Key;
    
    public string Notes => GetValue(x => x.Notes);
    public decimal Target => GetValue(x => x.Target);
    
    public string DonationFormEmbedCode => GetValue(x => x.DonationFormEmbedCode);
    public string DonationButtonEmbedCode => GetValue(x => x.DonationButtonEmbedCode);
    public string DonationPopupEmbedCode => GetValue(x => x.DonationPopupEmbedCode);

    public IEnumerable<OfferingContent> Offerings => Content().Descendants()
                                                              .Where(x => x.IsComposedOf(AliasHelper<OfferingContent>.ContentTypeAlias()))
                                                              .As<OfferingContent>();

    public OfferingContent DefaultOffering => Offerings.FirstOrDefault();
    
    public DonationFormContentContent FormContent { get; private set; }
    public QurbaniCampaignContent Qurbani { get; private set; }
    public ScheduledGivingCampaignContent ScheduledGiving { get; private set; }
    public StandardCampaignContent Standard { get; private set; }
    public TelethonCampaignContent Telethon { get; private set; }
    
    
    
    public CampaignType Type {
        get {
            if (Content().ContentType.Alias.EqualsInvariant(StandardCampaignAlias)) {
                return CampaignTypes.Standard;
            } else if (Content().ContentType.Alias.EqualsInvariant(QurbaniCampaignAlias)) {
                return CampaignTypes.Qurbani;
            } else if (Content().ContentType.Alias.EqualsInvariant(ScheduledGivingCampaignAlias)) {
                return CampaignTypes.ScheduledGiving;
            } else if (Content().ContentType.Alias.EqualsInvariant(TelethonCampaignAlias)) {
                return CampaignTypes.Telethon;
            } else {
                throw UnrecognisedValueException.For(Content().ContentType.Alias);
            }
        }
    }
}