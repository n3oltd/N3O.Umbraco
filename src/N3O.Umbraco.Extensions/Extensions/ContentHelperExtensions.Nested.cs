using N3O.Umbraco.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.PropertyEditors.ValueConverters;

namespace N3O.Umbraco.Extensions;

public static partial class ContentHelperExtensions {
    public static IPublishedElement GetNestedContent(this IContentHelper contentHelper, ElementsProperty property) {
        if (!property.Type.IsNestedContent()) {
            throw new Exception("Property is not nested content");
        }
        
        return GetNestedContent(contentHelper, property.ContentType.Alias, property.Type.Alias, property.Json);
    }
    
    public static IPublishedElement GetNestedContent(this IContentHelper contentHelper,
                                                     string contentTypeAlias,
                                                     IProperty property) {
        if (!property.PropertyType.IsNestedContent()) {
            throw new Exception("Property is not nested content");
        }
        
        return GetNestedContent(contentHelper, contentTypeAlias, property.PropertyType.Alias, property.GetValue());
    }
    
    public static IPublishedElement GetNestedContent(this IContentHelper contentHelper,
                                                     string contentTypeAlias,
                                                     string propertyTypeAlias,
                                                     object propertyValue) {
        var publishedElement = contentHelper.GetConvertedValue<NestedContentSingleValueConverter, IPublishedElement>(contentTypeAlias,
                                                                                                                    propertyTypeAlias,
                                                                                                                    propertyValue);

        return publishedElement;
    }
    
    public static IReadOnlyList<IPublishedElement> GetNestedContents(this IContentHelper contentHelper,
                                                                     ElementsProperty property) {
        if (!property.Type.IsNestedContent()) {
            throw new Exception("Property is not nested content");
        }
        
        return GetNestedContents(contentHelper, property.ContentType.Alias, property.Type.Alias, property.Json);
    }
    
    public static IReadOnlyList<IPublishedElement> GetNestedContents(this IContentHelper contentHelper,
                                                                     string contentTypeAlias,
                                                                     IProperty property) {
        if (!property.PropertyType.IsNestedContent()) {
            throw new Exception("Property is not nested content");
        }
        
        return GetNestedContents(contentHelper, contentTypeAlias, property.PropertyType.Alias, property.GetValue());
    }
    
    public static IReadOnlyList<IPublishedElement> GetNestedContents(this IContentHelper contentHelper,
                                                                     string contentTypeAlias,
                                                                     string propertyTypeAlias,
                                                                     object propertyValue) {
        if (propertyValue == null) {
            return new List<IPublishedElement>();
        }
        
        var publishedElements = contentHelper.GetConvertedValue<NestedContentManyValueConverter, IEnumerable<IPublishedElement>>(contentTypeAlias,
                                                                                                                                 propertyTypeAlias,
                                                                                                                                 propertyValue);

        return publishedElements.ToList();
    }
}
