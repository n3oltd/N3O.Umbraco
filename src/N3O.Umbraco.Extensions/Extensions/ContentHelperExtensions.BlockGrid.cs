using N3O.Umbraco.Content;
using System;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Models.Blocks;
using Umbraco.Cms.Core.PropertyEditors.ValueConverters;

namespace N3O.Umbraco.Extensions;

public static partial class ContentHelperExtensions {
    public static BlockGridModel GetBlockGrid(this IContentHelper contentHelper, ElementsProperty property) {
        if (!property.Type.IsBlockGrid()) {
            throw new Exception("Property is not block grid content");
        }
        
        return GetBlockGrid(contentHelper, property.ContentType.Alias, property.Type.Alias, property.Json);
    }
    
    public static BlockGridModel GetBlockGrid(this IContentHelper contentHelper,
                                              string contentTypeAlias,
                                              IProperty property) {
        if (!property.PropertyType.IsBlockGrid()) {
            throw new Exception("Property is not block grid content");
        }
        
        return GetBlockGrid(contentHelper, contentTypeAlias, property.PropertyType.Alias, property.GetValue());
    }
    
    public static BlockGridModel GetBlockGrid(this IContentHelper contentHelper,
                                              string contentTypeAlias,
                                              string propertyTypeAlias,
                                              object propertyValue) {
        var blockGridModel = contentHelper.GetConvertedValue<BlockGridPropertyValueConverter, BlockGridModel>(contentTypeAlias,
                                                                                                              propertyTypeAlias,
                                                                                                              propertyValue);

        return blockGridModel;
    }
}
