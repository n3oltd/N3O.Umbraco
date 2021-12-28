using N3O.Umbraco.Content;
using System;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Extensions;

public static class PublishedContentModelExtensions {
    public static T As<T>(this PublishedContentModel publishedContent) {
        if (publishedContent is T typedContent) {
            return typedContent;
        }

        if (typeof(T).ImplementsInterface<IUmbracoContent>()) {
            throw new Exception($"{typeof(T).GetFriendlyName()} does not implement {nameof(IUmbracoContent)}");
        }
        
        return ConvertTo<T>(publishedContent);
    }
    
    public static T To<T>(this PublishedContentModel publishedContent) where T : IUmbracoContent, new() {
        return ConvertTo<T>(publishedContent);
    }
    
    private static T ConvertTo<T>(this PublishedContentModel publishedContent) {
        var model = Activator.CreateInstance<T>();
        ((IUmbracoContent) model).Content = publishedContent;

        return model;
    }
}