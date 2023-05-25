using N3O.Umbraco.Content;
using System;
using System.Collections.Generic;
using Umbraco.Cms.Core.Models;
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
    
    public static IContent GetContentByUdi(this IContentService contentService, string udi) {
        var contentId = Guid.ParseExact(udi.Substring(udi.Length - 32), "N");
        var content = contentService.GetById(contentId);

        return content;
    }
}
