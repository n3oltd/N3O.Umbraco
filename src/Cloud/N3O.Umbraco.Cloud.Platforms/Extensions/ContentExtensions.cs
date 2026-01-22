using N3O.Umbraco.Cloud.Platforms.Content;
using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Services;

namespace N3O.Umbraco.Cloud.Platforms.Extensions;

public static class ContentExtensions {
    public static bool IsCampaign(this IContent content, IContentTypeService contentTypeService) {
        return HasComposition(contentTypeService, content, AliasHelper<CampaignContent>.ContentTypeAlias());
    }
    
    public static bool IsDonationButtonElement(this IContent content) {
        return content.ContentType.Alias.EqualsInvariant(AliasHelper<DonationButtonElementContent>.ContentTypeAlias());
    }
    
    public static bool IsDonationFormElement(this IContent content) {
        return content.ContentType.Alias.EqualsInvariant(AliasHelper<DonationFormElementContent>.ContentTypeAlias());
    }
    
    public static bool IsElement(this IContent content, IContentTypeService contentTypeService) {
        return HasComposition(contentTypeService, content, AliasHelper<ElementContent>.ContentTypeAlias());
    }
    
    public static bool IsFeed(this IContent content) {
        return content.ContentType.Alias == PlatformsConstants.Feeds.Feed.Alias;
    }
    
    public static bool IsFeeds(this IContent content) {
        return content.ContentType.Alias == PlatformsConstants.Feeds.Alias;
    }
    
    public static bool IsFeedItem(this IContent content, IContentTypeService contentTypeService) {
        return HasComposition(contentTypeService, content, PlatformsConstants.Feeds.Item.Alias);
    }
    
    public static bool IsOffering(this IContent content, IContentTypeService contentTypeService) {
        return HasComposition(contentTypeService, content, AliasHelper<OfferingContent>.ContentTypeAlias());
    }
    
    public static bool IsZakatCalculatorField(this IContent content) {
        return content.ContentType.Alias.EqualsInvariant(PlatformsConstants.Zakat.Settings.Calculator.Field.Alias);
    }
    
    public static bool IsZakatCalculatorSettings(this IContent content) {
        return content.ContentType.Alias.EqualsInvariant(PlatformsConstants.Zakat.Settings.Calculator.Alias);
    }

    private static bool HasComposition(IContentTypeService contentTypeService,
                                       IContent content,
                                       string compositionAlias) {
        var contentType = contentTypeService.Get(content.ContentTypeId);
        
        return contentType.CompositionAliases().Contains(compositionAlias, true);
    }
}