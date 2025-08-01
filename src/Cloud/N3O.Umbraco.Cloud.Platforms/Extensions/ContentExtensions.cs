using N3O.Umbraco.Cloud.Platforms.Content;
using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using System.Linq;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Services;
using Umbraco.Extensions;

namespace N3O.Umbraco.Cloud.Platforms.Extensions;

public static class ContentExtensions {
    public static bool IsCampaign(this IContent content, IContentTypeService contentTypeService) {
        return HasComposition(contentTypeService, content, AliasHelper<CampaignContent>.ContentTypeAlias());
    }
    
    public static bool IsDesignation(this IContent content, IContentTypeService contentTypeService) {
        return HasComposition(contentTypeService, content, AliasHelper<DesignationContent>.ContentTypeAlias());
    }
    
    public static bool IsDonateButtonElement(this IContent content) {
        return content.ContentType.Alias.EqualsInvariant(AliasHelper<DonateButtonElementContent>.ContentTypeAlias());
    }
    
    public static bool IsDonationFormElement(this IContent content) {
        return content.ContentType.Alias.EqualsInvariant(AliasHelper<DonationFormElementContent>.ContentTypeAlias());
    }
    
    public static bool IsFundStructure(this IContent content, IContentCache contentCache) {
        return IsSelfOrDescendantOfType<FundStructureContent>(contentCache, content);
    }
    
    public static bool IsElement(this IContent content, IContentTypeService contentTypeService) {
        return HasComposition(contentTypeService, content, AliasHelper<ElementContent>.ContentTypeAlias());
    }
    
    public static bool IsPlatformsCampaignOrDesignationOrElement(this IContent content, IContentTypeService contentTypeService) {
        return IsCampaign(content, contentTypeService) ||
               IsDesignation(content, contentTypeService) ||
               IsElement(content, contentTypeService);
    }
    
    public static bool IsPlatformsSubscriptionSettingContent(this IContent content, IContentCache contentCache) {
        return IsSelfOrDescendantOfType<SettingsContent>(contentCache, content);
    }

    private static bool HasComposition(IContentTypeService contentTypeService,
                                       IContent content,
                                       string compositionAlias) {
        var contentType = contentTypeService.Get(content.ContentTypeId);
        
        return contentType.CompositionAliases().Contains(compositionAlias, true);
    }
    
    private static bool IsSelfOrDescendantOfType<T>(IContentCache contentCache, IContent content) 
        where T : IUmbracoContent {
        var ancestorId = contentCache.Single<T>()?.Content().Id;

        if (!ancestorId.HasValue()) {
            return false;
        }
        
        var ancestorIdsOfContent = content.GetAncestorIds().OrEmpty().ToList();

        return ancestorIdsOfContent.Contains(ancestorId.GetValueOrThrow());
    }
}