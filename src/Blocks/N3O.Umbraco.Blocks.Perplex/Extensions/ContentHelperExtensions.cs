using N3O.Umbraco.Content;
using Perplex.ContentBlocks.PropertyEditor;
using System;
using Umbraco.Cms.Core.Models;

namespace N3O.Umbraco.Blocks.Perplex.Extensions;

public static class ContentHelperExtensions {
    public static global::Perplex.ContentBlocks.Rendering.ContentBlocks GetPerplexBlocks(this IContentHelper contentHelper,
                                                                                         NestedContentProperty property) {
        if (!property.Type.IsPerplexBlocks()) {
            throw new Exception("Property is not Perplex blocks");
        }
        
        return GetPerplexBlocks(contentHelper, property.ContentType.Alias, property.Type.Alias, property.Json);
    }
    
    public static global::Perplex.ContentBlocks.Rendering.ContentBlocks GetPerplexBlocks(this IContentHelper contentHelper,
                                                                                         string contentTypeAlias,
                                                                                         IProperty property) {
        if (!property.PropertyType.IsPerplexBlocks()) {
            throw new Exception("Property is not Perplex blocks");
        }
        
        return GetPerplexBlocks(contentHelper, contentTypeAlias, property.PropertyType.Alias, property.GetValue());
    }

    public static global::Perplex.ContentBlocks.Rendering.ContentBlocks GetPerplexBlocks(this IContentHelper contentHelper,
                                                                                         string contentTypeAlias,
                                                                                         string propertyTypeAlias,
                                                                                         object propertyValue) {
        if (propertyValue == null) {
            return null;
        }

        var contentBlocks = contentHelper.GetConvertedValue<ContentBlocksValueConverter, global::Perplex.ContentBlocks.Rendering.ContentBlocks>(contentTypeAlias,
                                                                                                                                                propertyTypeAlias,
                                                                                                                                                propertyValue);

        return contentBlocks;
    }
}
