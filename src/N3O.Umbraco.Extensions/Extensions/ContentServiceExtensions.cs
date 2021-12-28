using N3O.Umbraco.Content;
using System;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Services;

namespace N3O.Umbraco.Extensions;

public static class ContentServiceExtensions {
    public static IContent Create<T>(this IContentService contentService, string name, int parentId) {
        return contentService.Create(name, parentId, AliasHelper.ForContentType<T>());
    }
    
    public static IContent Create<T>(this IContentService contentService, string name, Guid parentId) {
        return contentService.Create(name, parentId, AliasHelper.ForContentType<T>());
    }
}
