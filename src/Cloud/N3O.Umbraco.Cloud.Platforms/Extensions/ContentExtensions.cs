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
    
    public static bool IsElement(this IContent content, IContentTypeService contentTypeService) {
        return HasComposition(contentTypeService, content, AliasHelper<ElementContent>.ContentTypeAlias());
    }
    
    public static bool IsPlatformsContent(this IContent content, IContentCache contentCache) {
        var platformsId = contentCache.Single<PlatformsContent>()?.Content().Id;

        if (!platformsId.HasValue()) {
            return false;
        }
        
        var ancestorIds = content.GetAncestorIds().OrEmpty().ToList();

        return ancestorIds.Contains(platformsId.GetValueOrThrow());
    }

    private static bool HasComposition(IContentTypeService contentTypeService,
                                       IContent content,
                                       string compositionAlias) {
        var contentType = contentTypeService.Get(content.ContentTypeId);
        
        return contentType.CompositionAliases().Contains(compositionAlias, true);
    }
}