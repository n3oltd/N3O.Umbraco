using N3O.Umbraco.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Cms.Core.Models;
using Umbraco.Community.Contentment.DataEditors;

namespace N3O.Umbraco.Extensions;

public static partial class ContentHelperExtensions {
    public static T GetDataListValue<T>(this IContentHelper contentHelper,
                                        ContentProperties contentProperties,
                                        string propertyTypeAlias) {
        var contentProperty = contentProperties.GetPropertyByAlias(propertyTypeAlias);
        
        return GetDataListValue<T>(contentHelper, contentProperty);
    }
    
    public static T GetDataListValue<T>(this IContentHelper contentHelper, IContentProperty property) {
        if (!property.Type.IsDataList()) {
            throw new Exception("Property is not data list");
        }
        
        return GetDataListValue<T>(contentHelper, property.ContentType.Alias, property.Type.Alias, property.Value);
    }
    
    public static T GetDataListValue<T>(this IContentHelper contentHelper,
                                        string contentTypeAlias,
                                        IProperty property) {
        if (!property.PropertyType.IsDataList()) {
            throw new Exception("Property is not data list");
        }
        
        return GetDataListValue<T>(contentHelper, contentTypeAlias, property.PropertyType.Alias, property.GetValue());
    }
    
    public static T GetDataListValue<T>(this IContentHelper contentHelper,
                                        string contentTypeAlias,
                                        string propertyTypeAlias,
                                        object propertyValue) {
        if (propertyValue is string strValue) {
            if (!strValue.HasValue() || strValue == "[]") {
                return default;
            }
        }
                                        
        return contentHelper.GetConvertedValue<DataListValueConverter, T>(contentTypeAlias,
                                                                          propertyTypeAlias,
                                                                          propertyValue);
    }
    
    public static IReadOnlyList<T> GetDataListValues<T>(this IContentHelper contentHelper,
                                                        ContentProperties contentProperties,
                                                        string propertyTypeAlias) {
        var contentProperty = contentProperties.GetPropertyByAlias(propertyTypeAlias);
        
        return GetDataListValues<T>(contentHelper, contentProperty);
    }

    public static IReadOnlyList<T> GetDataListValues<T>(this IContentHelper contentHelper, IContentProperty property) {
        if (!property.Type.IsDataList()) {
            throw new Exception("Property is not data list");
        }

        return GetDataListValues<T>(contentHelper, property.ContentType.Alias, property.Type.Alias, property.Value);
    }
    
    public static IReadOnlyList<T> GetDataListValues<T>(this IContentHelper contentHelper,
                                                        string contentTypeAlias,
                                                        IProperty property) {
        if (!property.PropertyType.IsDataList()) {
            throw new Exception("Property is not data list");
        }
        
        return GetDataListValues<T>(contentHelper, contentTypeAlias, property.PropertyType.Alias, property.GetValue());
    }

    public static IReadOnlyList<T> GetDataListValues<T>(this IContentHelper contentHelper,
                                                        string contentTypeAlias,
                                                        string propertyTypeAlias,
                                                        object propertyValue) {
        if (propertyValue is string strValue) {
            if (!strValue.HasValue() || strValue == "[]") {
                return new List<T>();
            }
        }
        
        var items = contentHelper.GetConvertedValue<DataListValueConverter, IEnumerable<T>>(contentTypeAlias,
                                                                                            propertyTypeAlias,
                                                                                            propertyValue);

        return items.OrEmpty().ToList();
    }
}
