using N3O.Umbraco.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.PropertyEditors.ValueConverters;

namespace N3O.Umbraco.Extensions;

public static partial class ContentHelperExtensions {
    public static string GetDropdownValue(this IContentHelper contentHelper,
                                          ContentProperties contentProperties,
                                          string propertyTypeAlias) {
        var contentProperty = contentProperties.GetPropertyByAlias(propertyTypeAlias);
        
        return GetDropdownValue(contentHelper, contentProperty);
    }
    
    public static string GetDropdownValue(this IContentHelper contentHelper, IContentProperty property) {
        if (!property.Type.IsDropdown()) {
            throw new Exception("Property is not dropdown");
        }
        
        return GetDropdownValue(contentHelper, property.ContentType.Alias, property.Type.Alias, property.Value);
    }
    
    public static string GetDropdownValue(this IContentHelper contentHelper,
                                          string contentTypeAlias,
                                          IProperty property) {
        if (!property.PropertyType.IsDropdown()) {
            throw new Exception("Property is not dropdown");
        }
        
        return GetDropdownValue(contentHelper, contentTypeAlias, property.PropertyType.Alias, property.GetValue());
    }
    
    public static string GetDropdownValue(this IContentHelper contentHelper,
                                          string contentTypeAlias,
                                          string propertyTypeAlias,
                                          object propertyValue) {
        if (propertyValue is string strValue) {
            if (!strValue.HasValue() || strValue == "[]") {
                return default;
            }
        }
                                        
        return contentHelper.GetConvertedValue<FlexibleDropdownPropertyValueConverter, string>(contentTypeAlias,
                                                                                               propertyTypeAlias,
                                                                                               propertyValue);
    }
    
    public static IReadOnlyList<string> GetDropdownValues(this IContentHelper contentHelper,
                                                          ContentProperties contentProperties,
                                                          string propertyTypeAlias) {
        var contentProperty = contentProperties.GetPropertyByAlias(propertyTypeAlias);
        
        return GetDropdownValues(contentHelper, contentProperty);
    }

    public static IReadOnlyList<string> GetDropdownValues(this IContentHelper contentHelper, IContentProperty property) {
        if (!property.Type.IsDropdown()) {
            throw new Exception("Property is not dropdown");
        }

        return GetDropdownValues(contentHelper, property.ContentType.Alias, property.Type.Alias, property.Value);
    }
    
    public static IReadOnlyList<string> GetDropdownValues(this IContentHelper contentHelper,
                                                          string contentTypeAlias,
                                                          IProperty property) {
        if (!property.PropertyType.IsDropdown()) {
            throw new Exception("Property is not dropdown");
        }
        
        return GetDropdownValues(contentHelper, contentTypeAlias, property.PropertyType.Alias, property.GetValue());
    }

    public static IReadOnlyList<string> GetDropdownValues(this IContentHelper contentHelper,
                                                          string contentTypeAlias,
                                                          string propertyTypeAlias,
                                                          object propertyValue) {
        if (propertyValue is string strValue) {
            if (!strValue.HasValue() || strValue == "[]") {
                return new List<string>();
            }
        }
        
        var items = contentHelper.GetConvertedValue<FlexibleDropdownPropertyValueConverter, IEnumerable<string>>(contentTypeAlias,
                                                                                                                 propertyTypeAlias,
                                                                                                                 propertyValue);

        return items.OrEmpty().ToList();
    }
}
