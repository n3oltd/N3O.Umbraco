using N3O.Umbraco.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Extensions;

namespace N3O.Umbraco.Extensions;

public static class PublishedContentExtensions {
    public static string AbsoluteUrl(this IPublishedContent content) {
        return content.Url(mode: UrlMode.Absolute);
    }
    
    public static T As<T>(this IPublishedContent publishedContent) {
        if (publishedContent is T typedContent) {
            return typedContent;
        }

        if (!typeof(T).ImplementsInterface<IUmbracoContent>()) {
            throw new Exception($"{typeof(T).GetFriendlyName()} does not implement {nameof(IUmbracoContent)}");
        }
    
        return ConvertTo<T>(publishedContent);
    }

    public static IReadOnlyList<T> As<T>(this IEnumerable<IPublishedContent> publishedContents) {
        return publishedContents.Select(x => x.As<T>()).ToList();
    }

    public static T Child<T>(this IPublishedContent content)
        where T : class, IPublishedContent {
        return Child<T>(content, _ => true);
    }

    public static T Child<T>(this IPublishedContent content, Func<T, bool> filter)
        where T : class, IPublishedContent {
        return Children(content, filter).FirstOrDefault();
    }

    public static IEnumerable<T> Children<T>(this IPublishedContent content)
        where T : class, IPublishedContent {
        return Children<T>(content, _ => true);
    }

    public static IEnumerable<T> Children<T>(this IPublishedContent content, Func<T, bool> filter)
        where T : class, IPublishedContent {
        return FriendlyPublishedContentExtensions.Children<T>(content).Where(filter);
    }

    public static T Descendant<T>(this IPublishedContent content) where T : class, IPublishedContent {
        return Descendant<T>(content, _ => true);
    }

    public static T Descendant<T>(this IPublishedContent content, Func<T, bool> filter)
        where T : class, IPublishedContent {
        return Descendants(content, filter).FirstOrDefault();
    }

    public static IEnumerable<T> Descendants<T>(this IPublishedContent content, Func<T, bool> filter)
        where T : class, IPublishedContent {
        return FriendlyPublishedContentExtensions.Descendants<T>(content).Where(filter);
    }

    public static bool HasTemplate(this IPublishedContent content) {
        return content.TemplateId != 0;
    }

    public static string RelativeUrl(this IPublishedContent content) {
        return content.Url(mode: UrlMode.Relative);
    }
    
    public static T To<T>(this IPublishedContent publishedContent) where T : IUmbracoContent, new() {
        return ConvertTo<T>(publishedContent);
    }
    
    private static T ConvertTo<T>(this IPublishedContent publishedContent) {
        if (publishedContent == null) {
            return default;
        }
        
        var model = Activator.CreateInstance<T>();
        ((IUmbracoContent) model).Content( publishedContent);

        return model;
    }
}
