using N3O.Umbraco.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.PropertyEditors.ValueConverters;

namespace N3O.Umbraco.Extensions;

public static partial class ContentHelperExtensions {
    public static MediaWithCrops GetMediaPickerValue(this IContentHelper contentHelper,
                                                     ContentProperties contentProperties,
                                                     string propertyTypeAlias) {
        var contentProperty = contentProperties.GetPropertyByAlias(propertyTypeAlias);
        
        return GetMediaPickerValue(contentHelper, contentProperty);
    }
    
    public static MediaWithCrops GetMediaPickerValue(this IContentHelper contentHelper, IContentProperty property) {
        if (!property.Type.IsMediaPicker()) {
            throw new Exception("Property is not a media picker");
        }
        
        return GetMediaPickerValue(contentHelper, property.ContentType.Alias, property.Type.Alias, property.Value);
    }
    
    public static MediaWithCrops GetMediaPickerValue(this IContentHelper contentHelper,
                                                     string contentTypeAlias,
                                                     IProperty property) {
        if (!property.PropertyType.IsMediaPicker()) {
            throw new Exception("Property is not a media picker");
        }
        
        return GetMediaPickerValue(contentHelper, contentTypeAlias, property.PropertyType.Alias, property.GetValue());
    }
    
    public static MediaWithCrops GetMediaPickerValue(this IContentHelper contentHelper,
                                                     string contentTypeAlias,
                                                     string propertyTypeAlias,
                                                     object propertyValue) {
        var item = contentHelper.GetConvertedValue<MediaPickerWithCropsValueConverter, MediaWithCrops>(contentTypeAlias,
                                                                                                       propertyTypeAlias,
                                                                                                       propertyValue);

        return item;
    }

    public static IReadOnlyList<MediaWithCrops> GetMediaPickerValues(this IContentHelper contentHelper,
                                                                     ContentProperties contentProperties,
                                                                     string propertyTypeAlias) {
        var contentProperty = contentProperties.GetPropertyByAlias(propertyTypeAlias);

        return GetMediaPickerValues(contentHelper, contentProperty);
    }

    public static IReadOnlyList<MediaWithCrops> GetMediaPickerValues(this IContentHelper contentHelper,
                                                                     IContentProperty property) {
        if (!property.Type.IsMediaPicker()) {
            throw new Exception("Property is not a media picker");
        }
        
        return GetMediaPickerValues(contentHelper, property.ContentType.Alias, property.Type.Alias, property.Value);
    }
    
    public static IReadOnlyList<MediaWithCrops> GetMediaPickerValues(this IContentHelper contentHelper,
                                                                     string contentTypeAlias,
                                                                     IProperty property) {
        if (!property.PropertyType.IsMediaPicker()) {
            throw new Exception("Property is not a media picker");
        }
        
        return GetMediaPickerValues(contentHelper, contentTypeAlias, property.PropertyType.Alias, property.GetValue());
    }

    public static IReadOnlyList<MediaWithCrops> GetMediaPickerValues(this IContentHelper contentHelper,
                                                                     string contentTypeAlias,
                                                                     string propertyTypeAlias,
                                                                     object propertyValue) {
        var items = contentHelper.GetConvertedValue<MediaPickerWithCropsValueConverter, IEnumerable<MediaWithCrops>>(contentTypeAlias,
                                                                                                                     propertyTypeAlias,
                                                                                                                     propertyValue);

        return items.ToList();
    }
}
