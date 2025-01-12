using N3O.Umbraco.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.PropertyEditors.ValueConverters;

namespace N3O.Umbraco.Extensions;

public static partial class ContentHelperExtensions {
    public static T GetMultiNodeTreePickerValue<T>(this IContentHelper contentHelper,
                                                   ContentProperties contentProperties,
                                                   string propertyTypeAlias) {
        var contentProperty = contentProperties.GetPropertyByAlias(propertyTypeAlias);
        
        return GetMultiNodeTreePickerValue<T>(contentHelper, contentProperty);
    }
    
    public static T GetMultiNodeTreePickerValue<T>(this IContentHelper contentHelper, IContentProperty property) {
        if (!property.Type.IsMultiNodeTreePicker()) {
            throw new Exception("Property is not a multi node tree picker");
        }
        
        return GetMultiNodeTreePickerValue<T>(contentHelper, property.ContentType.Alias, property.Type.Alias, property.Value);
    }
    
    public static T GetMultiNodeTreePickerValue<T>(this IContentHelper contentHelper,
                                                   string contentTypeAlias,
                                                   IProperty property) {
        if (!property.PropertyType.IsMultiNodeTreePicker()) {
            throw new Exception("Property is not a multi node tree picker");
        }
        
        return GetMultiNodeTreePickerValue<T>(contentHelper, contentTypeAlias, property.PropertyType.Alias, property.GetValue());
    }
    
    public static T GetMultiNodeTreePickerValue<T>(this IContentHelper contentHelper,
                                                   string contentTypeAlias,
                                                   string propertyTypeAlias,
                                                   object propertyValue) {
        var item = contentHelper.GetConvertedValue<MultiNodeTreePickerValueConverter, IPublishedContent>(contentTypeAlias,
                                                                                                         propertyTypeAlias,
                                                                                                         propertyValue);

        return (T) item;
    }

    public static IReadOnlyList<T> GetMultiNodeTreePickerValues<T>(this IContentHelper contentHelper,
                                                                   ContentProperties contentProperties,
                                                                   string propertyTypeAlias) {
        var contentProperty = contentProperties.GetPropertyByAlias(propertyTypeAlias);

        return GetMultiNodeTreePickerValues<T>(contentHelper, contentProperty);
    }

    public static IReadOnlyList<T> GetMultiNodeTreePickerValues<T>(this IContentHelper contentHelper, IContentProperty property) {
        if (!property.Type.IsMultiNodeTreePicker()) {
            throw new Exception("Property is not a multi node tree picker");
        }
        
        return GetMultiNodeTreePickerValues<T>(contentHelper, property.ContentType.Alias, property.Type.Alias, property.Value);
    }
    
    public static IReadOnlyList<T> GetMultiNodeTreePickerValues<T>(this IContentHelper contentHelper,
                                                                   string contentTypeAlias,
                                                                   IProperty property) {
        if (!property.PropertyType.IsMultiNodeTreePicker()) {
            throw new Exception("Property is not a multi node tree picker");
        }
        
        return GetMultiNodeTreePickerValues<T>(contentHelper, contentTypeAlias, property.PropertyType.Alias, property.GetValue());
    }

    public static IReadOnlyList<T> GetMultiNodeTreePickerValues<T>(this IContentHelper contentHelper,
                                                                   string contentTypeAlias,
                                                                   string propertyTypeAlias,
                                                                   object propertyValue) {
        var items = contentHelper.GetConvertedValue<MultiNodeTreePickerValueConverter, IEnumerable<IPublishedContent>>(contentTypeAlias,
                                                                                                                       propertyTypeAlias,
                                                                                                                       propertyValue);

        return items.OrEmpty().Cast<T>().ToList();
    }
}
