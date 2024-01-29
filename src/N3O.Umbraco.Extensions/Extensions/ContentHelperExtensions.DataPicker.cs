using N3O.Umbraco.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Cms.Core.Models;
using Umbraco.Community.Contentment.DataEditors;

namespace N3O.Umbraco.Extensions;

public static partial class ContentHelperExtensions {
    public static T GetDataPickerValue<T>(this IContentHelper contentHelper,
                                          ContentProperties contentProperties,
                                          string propertyTypeAlias) {
        var contentProperty = contentProperties.GetPropertyByAlias(propertyTypeAlias);
        
        return GetDataPickerValue<T>(contentHelper, contentProperty);
    }
    
    public static T GetDataPickerValue<T>(this IContentHelper contentHelper, IContentProperty property) {
        if (!property.Type.IsDataPicker()) {
            throw new Exception("Property is not a data picker");
        }
        
        return GetDataPickerValue<T>(contentHelper, property.ContentType.Alias, property.Type.Alias, property.Value);
    }
    
    public static T GetDataPickerValue<T>(this IContentHelper contentHelper,
                                          string contentTypeAlias,
                                          IProperty property) {
        if (!property.PropertyType.IsDataPicker()) {
            throw new Exception("Property is not a data picker");
        }
        
        return GetDataPickerValue<T>(contentHelper, contentTypeAlias, property.PropertyType.Alias, property.GetValue());
    }
    
    public static T GetDataPickerValue<T>(this IContentHelper contentHelper,
                                          string contentTypeAlias,
                                          string propertyTypeAlias,
                                          object propertyValue) {
        if (propertyValue is string strValue) {
            if (!strValue.HasValue() || strValue == "[]") {
                return default;
            }
        }

        var type = GetConverterType();
                                        
        return contentHelper.GetConvertedValue<T>(type,
                                                  contentTypeAlias,
                                                  propertyTypeAlias,
                                                  propertyValue);
    }
    
    public static IReadOnlyList<T> GetDataPickerValues<T>(this IContentHelper contentHelper,
                                                          ContentProperties contentProperties,
                                                          string propertyTypeAlias) {
        var contentProperty = contentProperties.GetPropertyByAlias(propertyTypeAlias);
        
        return GetDataPickerValues<T>(contentHelper, contentProperty);
    }

    public static IReadOnlyList<T> GetDataPickerValues<T>(this IContentHelper contentHelper,
                                                          IContentProperty property) {
        if (!property.Type.IsDataPicker()) {
            throw new Exception("Property is not a data picker");
        }

        return GetDataPickerValues<T>(contentHelper, property.ContentType.Alias, property.Type.Alias, property.Value);
    }
    
    public static IReadOnlyList<T> GetDataPickerValues<T>(this IContentHelper contentHelper,
                                                          string contentTypeAlias,
                                                          IProperty property) {
        if (!property.PropertyType.IsDataPicker()) {
            throw new Exception("Property is not a data picker");
        }
        
        return GetDataPickerValues<T>(contentHelper, contentTypeAlias, property.PropertyType.Alias, property.GetValue());
    }

    public static IReadOnlyList<T> GetDataPickerValues<T>(this IContentHelper contentHelper,
                                                          string contentTypeAlias,
                                                          string propertyTypeAlias,
                                                          object propertyValue) {
        if (propertyValue is string strValue) {
            if (!strValue.HasValue() || strValue == "[]") {
                return new List<T>();
            }
        }

        var converterType = GetConverterType();
        
        var items = contentHelper.GetConvertedValue<IEnumerable<T>>(converterType,
                                                                    contentTypeAlias,
                                                                    propertyTypeAlias,
                                                                    propertyValue);

        return items.OrEmpty().ToList();
    }

    // DataPickerValueConverter is marked as internal
    private static Type GetConverterType() {
        return typeof(DataListValueConverter).Assembly.GetType("Umbraco.Community.Contentment.DataEditors.DataPickerValueConverter");
    }
}
