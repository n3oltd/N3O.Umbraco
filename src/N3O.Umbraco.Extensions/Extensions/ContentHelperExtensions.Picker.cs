using N3O.Umbraco.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.PropertyEditors.ValueConverters;

namespace N3O.Umbraco.Extensions;

public static partial class ContentHelperExtensions {
    public static T GetPickerValue<T>(this IContentHelper contentHelper, IContentProperty property) {
        if (!property.Type.IsPicker()) {
            throw new Exception("Property is not picker");
        }
        
        return GetPickerValue<T>(contentHelper, property.ContentType.Alias, property.Type.Alias, property.Value);
    }
    
    public static T GetPickerValue<T>(this IContentHelper contentHelper,
                                      string contentTypeAlias,
                                      IProperty property) {
        if (!property.PropertyType.IsPicker()) {
            throw new Exception("Property is not picker");
        }
        
        return GetPickerValue<T>(contentHelper, contentTypeAlias, property.PropertyType.Alias, property.GetValue());
    }
    
    public static T GetPickerValue<T>(this IContentHelper contentHelper,
                                      string contentTypeAlias,
                                      string propertyTypeAlias,
                                      object propertyValue) {
        var item = contentHelper.GetConvertedValue<MultiNodeTreePickerValueConverter, IPublishedContent>(contentTypeAlias,
                                                                                                         propertyTypeAlias,
                                                                                                         propertyValue);

        return (T) item;
    }

    public static IReadOnlyList<T> GetPickerValues<T>(this IContentHelper contentHelper, IContentProperty property) {
        if (!property.Type.IsPicker()) {
            throw new Exception("Property is not picker");
        }
        
        return GetPickerValues<T>(contentHelper, property.ContentType.Alias, property.Type.Alias, property.Value);
    }
    
    public static IReadOnlyList<T> GetPickerValues<T>(this IContentHelper contentHelper,
                                                      string contentTypeAlias,
                                                      IProperty property) {
        if (!property.PropertyType.IsPicker()) {
            throw new Exception("Property is not picker");
        }
        
        return GetPickerValues<T>(contentHelper, contentTypeAlias, property.PropertyType.Alias, property.GetValue());
    }

    public static IReadOnlyList<T> GetPickerValues<T>(this IContentHelper contentHelper,
                                                      string contentTypeAlias,
                                                      string propertyTypeAlias,
                                                      object propertyValue) {
        var items = contentHelper.GetConvertedValue<MultiNodeTreePickerValueConverter, IEnumerable<IPublishedContent>>(contentTypeAlias,
                                                                                                                       propertyTypeAlias,
                                                                                                                       propertyValue);

        return items.OrEmpty().Cast<T>().ToList();
    }
}
