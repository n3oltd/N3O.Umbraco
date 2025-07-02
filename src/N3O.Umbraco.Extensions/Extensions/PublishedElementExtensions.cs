using N3O.Umbraco.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Extensions;

public static class PublishedElementExtensions {
    public static T As<T>(this IPublishedElement publishedElement,
                          IPublishedContent parent = null,
                          VariationContext variationContext = null) {
        if (publishedElement is T typedContent) {
            return typedContent;
        }

        if (!typeof(T).ImplementsInterface<IUmbracoElement>()) {
            throw new Exception($"{typeof(T).GetFriendlyName()} does not implement {nameof(IUmbracoElement)}");
        }
    
        return ConvertTo<T>(publishedElement, parent, variationContext);
    }

    public static IReadOnlyList<T> As<T>(this IEnumerable<IPublishedElement> publishedElements,
                                         IPublishedContent parent = null,
                                         VariationContext variationContext = null) {
        return publishedElements.Select(x => x.As<T>(parent, variationContext)).ToList();
    }

    public static T To<T>(this IPublishedElement publishedElement,
                          IPublishedContent parent = null,
                          VariationContext variationContext = null)
        where T : IUmbracoContent, new() {
        return ConvertTo<T>(publishedElement, parent, variationContext);
    }
    
    private static T ConvertTo<T>(this IPublishedElement publishedElement,
                                  IPublishedContent parent,
                                  VariationContext variationContext = null) {
        var model = Activator.CreateInstance<T>();

        if (model is IUmbracoElement umbracoElement) {
            umbracoElement.SetContent(publishedElement, parent);

            if (variationContext != null) {
                umbracoElement.SetVariationContext(variationContext);
            }
        }
        
        return model;
    }
}