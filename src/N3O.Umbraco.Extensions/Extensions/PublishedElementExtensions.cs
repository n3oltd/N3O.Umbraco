using N3O.Umbraco.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Extensions;

public static class PublishedElementExtensions {
    public static T As<T>(this IPublishedElement publishedElement, IPublishedContent parent) {
        if (publishedElement is T typedContent) {
            return typedContent;
        }

        if (!typeof(T).ImplementsInterface<IUmbracoElement>()) {
            throw new Exception($"{typeof(T).GetFriendlyName()} does not implement {nameof(IUmbracoElement)}");
        }
    
        return ConvertTo<T>(publishedElement, parent);
    }

    public static IReadOnlyList<T> As<T>(this IEnumerable<IPublishedElement> publishedElements, IPublishedContent parent) {
        return publishedElements.Select(x => x.As<T>(parent)).ToList();
    }

    public static T To<T>(this IPublishedElement publishedElement, IPublishedContent parent) where T : IUmbracoContent, new() {
        return ConvertTo<T>(publishedElement, parent);
    }
    
    private static T ConvertTo<T>(this IPublishedElement publishedElement, IPublishedContent parent) {
        var model = Activator.CreateInstance<T>();
        
        ((IUmbracoElement) model).Content(publishedElement, parent);

        return model;
    }
}