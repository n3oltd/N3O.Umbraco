using N3O.Umbraco.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Scoping;
using Umbraco.Cms.Core.Services;

namespace N3O.Umbraco.Extensions;

public static class ContentServiceExtensions {
    private static readonly string SettingsAlias = "settings";
    
    public static IContent Create<T>(this IContentService contentService, string name, int parentId) {
        return contentService.Create(name, parentId, AliasHelper<T>.ContentTypeAlias());
    }

    public static IContent Create<T>(this IContentService contentService, string name, Guid parentId) {
        return contentService.Create(name, parentId, AliasHelper<T>.ContentTypeAlias());
    }

    public static IEnumerable<IContent> GetAncestorsOrSelf(this IContentService contentService, IContent content) {
        var list = new List<IContent>();
        
        list.Add(content);
        
        if (content.ParentId > 0) {
            list.Add(contentService.GetById(content.ParentId));
            list.AddRange(contentService.GetAncestors(content.ParentId));
        }

        return list;
    }
    
    public static IEnumerable<IContent> GetDescendantsForContentOfType(this IContentService contentService,
                                                                       IContentTypeService contentTypeService,
                                                                       ICoreScopeProvider coreScopeProvider,
                                                                       IContent content,
                                                                       string contentTypeAlias) {
        var contentType = contentTypeService.Get(contentTypeAlias);
 
        var query = coreScopeProvider.CreateQuery<IContent>().Where(x => x.ContentTypeId == contentType.Id);
        
        var descendants = contentService.GetPagedDescendants(content.Id, 0, int.MaxValue, out _, query);
        
        return descendants;
    }
    
    public static IEnumerable<IContent> GetRootContents(this IContentService contentService) {
        return contentService.GetRootContent();
    }
    
    public static IContent GetRootSettings(this IContentService contentService) {
        return GetRootContents(contentService).Single(x => x.ContentType.Alias == SettingsAlias);
    }
}
