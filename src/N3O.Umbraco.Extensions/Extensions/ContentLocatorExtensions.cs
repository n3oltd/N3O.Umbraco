using N3O.Umbraco.Content;
using N3O.Umbraco.Utilities;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Extensions;

public static class ContentLocatorExtensions {
    public static IPublishedContent Special(this IContentLocator contentLocator, SpecialContent specialContent) {
        return contentLocator.Single<UrlSettingsContent>().GetSpecialContent(specialContent.UrlSettingsPropertyAlias);
    }

    public static T Special<T>(this IContentLocator contentLocator, SpecialContent specialContent) {
        return Special(contentLocator, specialContent).As<T>();
    }
}