using N3O.Umbraco.Content;
using Perplex.ContentBlocks.PropertyEditor;
using Perplex.ContentBlocks.Rendering;
using System;
using Umbraco.Cms.Core.Models;

namespace N3O.Umbraco.Extensions;

public static partial class ContentHelperExtensions {
    public static ContentBlocks GetContentBlocks(this IContentHelper contentHelper, NestedContentProperty property) {
        if (!property.Type.IsContentBlocks()) {
            throw new Exception("Property is not content blocks");
        }
        
        return GetContentBlocks(contentHelper, property.ContentType.Alias, property.Type.Alias, property.Json);
    }
    
    public static ContentBlocks GetContentBlocks(this IContentHelper contentHelper,
                                                 string contentTypeAlias,
                                                 IProperty property) {
        if (!property.PropertyType.IsContentBlocks()) {
            throw new Exception("Property is not content blocks");
        }
        
        return GetContentBlocks(contentHelper, contentTypeAlias, property.PropertyType.Alias, property.GetValue());
    }

    public static ContentBlocks GetContentBlocks(this IContentHelper contentHelper,
                                                 string contentTypeAlias,
                                                 string propertyTypeAlias,
                                                 object propertyValue) {
        if (propertyValue == null) {
            return null;
        }

        var contentBlocks = contentHelper.GetConvertedValue<ContentBlocksValueConverter, ContentBlocks>(contentTypeAlias,
                                                                                                        propertyTypeAlias,
                                                                                                        propertyValue);

        return contentBlocks;
    }
}
