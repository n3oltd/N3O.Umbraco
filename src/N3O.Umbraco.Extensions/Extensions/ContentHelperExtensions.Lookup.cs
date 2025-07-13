using N3O.Umbraco.Content;
using N3O.Umbraco.Lookups;
using System;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.PropertyEditors.ValueConverters;

namespace N3O.Umbraco.Extensions;

public static partial class ContentHelperExtensions {
    public static T GetLookupValue<T>(this IContentHelper contentHelper,
                                      ILookups lookups,
                                      ContentProperties contentProperties,
                                      string propertyTypeAlias)
        where T : ILookup {
        var contentProperty = contentProperties.GetPropertyByAlias(propertyTypeAlias);
        
        return GetLookupValue<T>(contentHelper, lookups, contentProperty);
    }
    
    public static T GetLookupValue<T>(this IContentHelper contentHelper, ILookups lookups, IContentProperty property)
        where T : ILookup {
        if (!property.Type.IsMultiNodeTreePicker() && !property.Type.IsDataList()) {
            throw new Exception("Property is not a multi node tree picker or datalist");
        }

        return GetLookupValue<T>(contentHelper,
                                 lookups,
                                 property.ContentType.Alias,
                                 property.Type.Alias,
                                 property.Value);
    }
    
    public static T GetLookupValue<T>(this IContentHelper contentHelper,
                                      ILookups lookups,
                                      string contentTypeAlias,
                                      IProperty property)
        where T : ILookup {
        if (!property.PropertyType.IsMultiNodeTreePicker() && !property.PropertyType.IsDataList()) {
            throw new Exception("Property is not a multi node tree picker or datalist");
        }

        return GetLookupValue<T>(contentHelper,
                                 lookups,
                                 contentTypeAlias,
                                 property.PropertyType.Alias,
                                 property.GetValue());
    }
    
    public static T GetLookupValue<T>(this IContentHelper contentHelper,
                                      ILookups lookups,
                                      string contentTypeAlias,
                                      string propertyTypeAlias,
                                      object propertyValue)
        where T : ILookup {
        var item = default(T);
        var isDataList = false;

        try {
            var publishedContent = contentHelper.GetConvertedValue<MultiNodeTreePickerValueConverter, IPublishedContent>(contentTypeAlias,
                                                                                                                         propertyTypeAlias,
                                                                                                                         propertyValue);

            item = lookups.FindById<T>(LookupContent.GetId(publishedContent));
        } catch {
            isDataList = true;
        }

        if (isDataList) {
            item = contentHelper.GetDataListValue<T>(contentTypeAlias, propertyTypeAlias, propertyValue);
        }

        return item;
    }

    public static IReadOnlyList<T> GetLookupValues<T>(this IContentHelper contentHelper,
                                                      ILookups lookups,
                                                      ContentProperties contentProperties,
                                                      string propertyTypeAlias)
        where T : ILookup {
        var contentProperty = contentProperties.GetPropertyByAlias(propertyTypeAlias);

        return GetLookupValues<T>(contentHelper, lookups, contentProperty);
    }

    public static IReadOnlyList<T> GetLookupValues<T>(this IContentHelper contentHelper,
                                                      ILookups lookups,
                                                      IContentProperty property)
        where T : ILookup {
        if (!property.Type.IsMultiNodeTreePicker() && !property.Type.IsDataList()) {
            throw new Exception("Property is not a multi node tree picker or datalist");
        }

        return GetLookupValues<T>(contentHelper,
                                  lookups,
                                  property.ContentType.Alias,
                                  property.Type.Alias,
                                  property.Value);
    }
    
    public static IReadOnlyList<T> GetLookupValues<T>(this IContentHelper contentHelper,
                                                      ILookups lookups,
                                                      string contentTypeAlias,
                                                      IProperty property)
        where T : ILookup {
        if (!property.PropertyType.IsMultiNodeTreePicker() && !property.PropertyType.IsDataList()) {
            throw new Exception("Property is not a multi node tree picker or datalist");
        }

        return GetLookupValues<T>(contentHelper,
                                  lookups,
                                  contentTypeAlias,
                                  property.PropertyType.Alias,
                                  property.GetValue());
    }

    public static IReadOnlyList<T> GetLookupValues<T>(this IContentHelper contentHelper,
                                                     ILookups lookups,
                                                     string contentTypeAlias,
                                                     string propertyTypeAlias,
                                                     object propertyValue)
        where T : ILookup {
        var items = new List<T>();
        var isDataList = false;

        try {
            var publishedContents = contentHelper.GetConvertedValue<MultiNodeTreePickerValueConverter, IEnumerable<IPublishedContent>>(contentTypeAlias,
                                                                                                                                       propertyTypeAlias,
                                                                                                                                       propertyValue);

            items.AddRange(publishedContents.Select(x => lookups.FindById<T>(LookupContent.GetId(x))));
        } catch {
            isDataList = true;
        }

        if (isDataList) {
            items.AddRange(contentHelper.GetDataListValues<T>(contentTypeAlias, propertyTypeAlias, propertyValue));
        }

        return items;
    }
}
