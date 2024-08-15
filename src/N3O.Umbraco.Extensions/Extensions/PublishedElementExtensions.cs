using N3O.Umbraco.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Extensions;

public static class PublishedElementExtensions {
    public static T As<T>(this IPublishedElement publishedElement) {
        if (publishedElement is T typedContent) {
            return typedContent;
        }

        if (!typeof(T).ImplementsInterface<IUmbracoElement>()) {
            throw new Exception($"{typeof(T).GetFriendlyName()} does not implement {nameof(IUmbracoElement)}");
        }
    
        return ConvertTo<T>(publishedElement);
    }

    public static IReadOnlyList<T> As<T>(this IEnumerable<IPublishedElement> publishedElements) {
        return publishedElements.Select(x => x.As<T>()).ToList();
    }

    public static T To<T>(this IPublishedElement publishedElement) where T : IUmbracoContent, new() {
        return ConvertTo<T>(publishedElement);
    }
    
    private static T ConvertTo<T>(this IPublishedElement publishedElement) {
        var model = Activator.CreateInstance<T>();
        
        ((IUmbracoElement) model).Content(publishedElement);

        return model;
    }
}
