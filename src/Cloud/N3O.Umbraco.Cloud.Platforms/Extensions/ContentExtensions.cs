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
        var contentType = contentTypeService.Get(content.ContentTypeId);
        
        return HasComposition(contentType, AliasHelper<CampaignContent>.ContentTypeAlias());
    }
    
    public static bool IsDesignation(this IContent content, IContentTypeService contentTypeService) {
        var contentType = contentTypeService.Get(content.ContentTypeId);
        
        return HasComposition(contentType, AliasHelper<DesignationContent>.ContentTypeAlias());
    }
    
    public static bool IsElement(this IContent content, IContentTypeService contentTypeService) {
        var contentType = contentTypeService.Get(content.ContentTypeId);
        
        return HasComposition(contentType, AliasHelper<ElementContent>.ContentTypeAlias());
    }
    
    public static bool IsPlatformEntity(this IContent content, IContentLocator contentLocator) {
        var platforms = contentLocator.Single<PlatformsContent>();
        
        var ancestorIds = content.GetAncestorIds().OrEmpty().ToList();

        return ancestorIds.Contains(platforms.Content().Id);
    }

    private static bool HasComposition(IContentType contentType, string compositionAlias) {
        return contentType.CompositionAliases().Contains(compositionAlias, true);
    }
}