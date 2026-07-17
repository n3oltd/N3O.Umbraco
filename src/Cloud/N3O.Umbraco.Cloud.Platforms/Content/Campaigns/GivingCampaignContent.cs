using N3O.Umbraco.Attributes;
using N3O.Umbraco.Cloud.Platforms.Clients;
using N3O.Umbraco.Content;
using N3O.Umbraco.Exceptions;
using N3O.Umbraco.Extensions;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Cloud.Platforms.Content;

[UmbracoContent(PlatformsConstants.Campaigns.CompositionAlias)]
public class GivingCampaignContent : UmbracoContent<GivingCampaignContent> {
    private static readonly string ScheduledGivingCampaignAlias = AliasHelper<ScheduledGivingCampaignContent>.ContentTypeAlias();
    private static readonly string RegularGivingCampaignAlias = AliasHelper<RegularGivingCampaignContent>.ContentTypeAlias();
    
    public override void SetContent(IPublishedContent content) {
        base.SetContent(content);
        
        if (Type == GivingType.Regular) {
            RegularGiving = new RegularGivingCampaignContent();
            RegularGiving.SetContent(content);
        } else if (Type == GivingType.Scheduled) {
            ScheduledGiving = new ScheduledGivingCampaignContent();
            ScheduledGiving.SetContent(content);
        } else {
            throw UnrecognisedValueException.For(Type);
        }
    }
    
    public override void SetVariationContext(VariationContext variationContext) {
        base.SetVariationContext(variationContext);

        RegularGiving?.SetVariationContext(variationContext);
        ScheduledGiving?.SetVariationContext(variationContext);
    }
    
    public ScheduledGivingCampaignContent ScheduledGiving { get; private set; }
    public RegularGivingCampaignContent RegularGiving { get; private set; }
    
    public GivingType Type {
        get {
            if (Content().ContentType.Alias.EqualsInvariant(RegularGivingCampaignAlias)) {
                return GivingType.Regular;
            } else if (Content().ContentType.Alias.EqualsInvariant(ScheduledGivingCampaignAlias)) {
                return GivingType.Scheduled;
            } else {
                throw UnrecognisedValueException.For(Content().ContentType.Alias);
            }
        }
    }
}