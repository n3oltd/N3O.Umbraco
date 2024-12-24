using N3O.Umbraco.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.PropertyEditors.ValueConverters;

namespace N3O.Umbraco.Extensions;

public static partial class ContentHelperExtensions {
    public static IPublishedElement GetBlockGrid(this IContentHelper contentHelper, ElementsProperty property) {
        if (!property.Type.IsBlockGrid()) {
            throw new Exception("Property is not block grid content");
        }
        
        return GetBlockGrid(contentHelper, property.ContentType.Alias, property.Type.Alias, property.Json);
    }
    
    public static IPublishedElement GetBlockGrid(this IContentHelper contentHelper,
                                                 string contentTypeAlias,
                                                 IProperty property) {
        if (!property.PropertyType.IsBlockGrid()) {
            throw new Exception("Property is not block grid content");
        }
        
        return GetBlockGrid(contentHelper, contentTypeAlias, property.PropertyType.Alias, property.GetValue());
    }
    
    public static IPublishedElement GetBlockGrid(this IContentHelper contentHelper,
                                                 string contentTypeAlias,
                                                 string propertyTypeAlias,
                                                 object propertyValue) {
        var publishedElement = contentHelper.GetConvertedValue<BlockGridPropertyValueConverter, IPublishedElement>(contentTypeAlias,
                                                                                                                   propertyTypeAlias,
                                                                                                                   propertyValue);

        return publishedElement;
    }
    
    public static IReadOnlyList<IPublishedElement> GetBlockGrids(this IContentHelper contentHelper,
                                                                 ElementsProperty property) {
        if (!property.Type.IsBlockGrid()) {
            throw new Exception("Property is not block grid content");
        }
        
        return GetBlockGrids(contentHelper, property.ContentType.Alias, property.Type.Alias, property.Json);
    }
    
    public static IReadOnlyList<IPublishedElement> GetBlockGrids(this IContentHelper contentHelper,
                                                                 string contentTypeAlias,
                                                                 IProperty property) {
        if (!property.PropertyType.IsBlockGrid()) {
            throw new Exception("Property is not block grid content");
        }
        
        return GetBlockGrids(contentHelper, contentTypeAlias, property.PropertyType.Alias, property.GetValue());
    }
    
    public static IReadOnlyList<IPublishedElement> GetBlockGrids(this IContentHelper contentHelper,
                                                                 string contentTypeAlias,
                                                                 string propertyTypeAlias,
                                                                 object propertyValue) {
        if (propertyValue == null) {
            return new List<IPublishedElement>();
        }
        
        var publishedElements = contentHelper.GetConvertedValue<BlockGridPropertyValueConverter, IEnumerable<IPublishedElement>>(contentTypeAlias,
                                                                                                                                 propertyTypeAlias,
                                                                                                                                 propertyValue);

        return publishedElements.ToList();
    }
}
