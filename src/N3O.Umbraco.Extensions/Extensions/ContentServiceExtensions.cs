using N3O.Umbraco.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Scoping;
using Umbraco.Cms.Core.Services;

namespace N3O.Umbraco.Extensions;

public static class ContentServiceExtensions {
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
                                                                       int contentId,
                                                                       string contentTypeAlias) {
        var contentType = contentTypeService.Get(contentTypeAlias);
 
        var query = coreScopeProvider.CreateQuery<IContent>().Where(x => x.ContentTypeId == contentType.Id);
        
        var descendants = contentService.GetPagedDescendants(contentId, 0, int.MaxValue, out _, query);
        
        return descendants;
    }

    public static IContent GetSettingContent(this IContentService contentService,
                                             IContentTypeService contentTypeService,
                                             ICoreScopeProvider coreScopeProvider,
                                             string contentTypeAlias) {
        var settingsRoot = GetSettingsRoot(contentService);

        if (settingsRoot == null) {
            return null;
        }

        var settingContents = GetDescendantsForContentOfType(contentService,
                                                             contentTypeService,
                                                             coreScopeProvider,
                                                             settingsRoot.Id,
                                                             contentTypeAlias);

        return settingContents.SingleOrDefault();
    }

    public static IContent GetSettingsRoot(this IContentService contentService) {
        return contentService.GetRootContent().Single(x => x.ContentType.Alias.EqualsInvariant("settings"));
    }
}
