using N3O.Umbraco.Content;
using N3O.Umbraco.Utilities;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Extensions;

public static class ContentCacheExtensions {
    public static IPublishedContent Special(this IContentCache contentCache, SpecialContent specialContent) {
        return contentCache.Single<UrlSettingsContent>()?.GetSpecialContent(specialContent.UrlSettingsPropertyAlias)
               // TODO Remove me once all sites migrated
               ?? contentCache.Single(specialContent.Id);
    }

    public static T Special<T>(this IContentCache contentCache, SpecialContent specialContent) {
        return Special(contentCache, specialContent).As<T>();
    }
}