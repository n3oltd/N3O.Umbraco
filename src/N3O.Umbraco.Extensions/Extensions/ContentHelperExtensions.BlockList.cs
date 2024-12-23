using N3O.Umbraco.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.PropertyEditors.ValueConverters;

namespace N3O.Umbraco.Extensions;

public static partial class ContentHelperExtensions {
    public static IPublishedElement GetBlockList(this IContentHelper contentHelper, ElementProperty property) {
        if (!property.Type.IsBlockList()) {
            throw new Exception("Property is not block list content");
        }
        
        return GetBlockList(contentHelper, property.ContentType.Alias, property.Type.Alias, property.Json);
    }
    
    public static IPublishedElement GetBlockList(this IContentHelper contentHelper,
                                                 string contentTypeAlias,
                                                 IProperty property) {
        if (!property.PropertyType.IsBlockList()) {
            throw new Exception("Property is not block list content");
        }
        
        return GetBlockList(contentHelper, contentTypeAlias, property.PropertyType.Alias, property.GetValue());
    }
    
    public static IPublishedElement GetBlockList(this IContentHelper contentHelper,
                                                 string contentTypeAlias,
                                                 string propertyTypeAlias,
                                                 object propertyValue) {
        var publishedElement = contentHelper.GetConvertedValue<BlockListPropertyValueConverter, IPublishedElement>(contentTypeAlias,
                                                                                                                   propertyTypeAlias,
                                                                                                                   propertyValue);

        return publishedElement;
    }
    
    public static IReadOnlyList<IPublishedElement> GetBlockLists(this IContentHelper contentHelper,
                                                                 ElementProperty property) {
        if (!property.Type.IsBlockList()) {
            throw new Exception("Property is not block list content");
        }
        
        return GetBlockLists(contentHelper, property.ContentType.Alias, property.Type.Alias, property.Json);
    }
    
    public static IReadOnlyList<IPublishedElement> GetBlockLists(this IContentHelper contentHelper,
                                                                 string contentTypeAlias,
                                                                 IProperty property) {
        if (!property.PropertyType.IsBlockList()) {
            throw new Exception("Property is not block list content");
        }
        
        return GetBlockLists(contentHelper, contentTypeAlias, property.PropertyType.Alias, property.GetValue());
    }
    
    public static IReadOnlyList<IPublishedElement> GetBlockLists(this IContentHelper contentHelper,
                                                                 string contentTypeAlias,
                                                                 string propertyTypeAlias,
                                                                 object propertyValue) {
        if (propertyValue == null) {
            return new List<IPublishedElement>();
        }
        
        var publishedElements = contentHelper.GetConvertedValue<BlockListPropertyValueConverter, IEnumerable<IPublishedElement>>(contentTypeAlias,
                                                                                                                                 propertyTypeAlias,
                                                                                                                                 propertyValue);

        return publishedElements.ToList();
    }
}
