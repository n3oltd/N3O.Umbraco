using N3O.Umbraco.Content;
using System;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Models.Blocks;
using Umbraco.Cms.Core.PropertyEditors.ValueConverters;

namespace N3O.Umbraco.Extensions;

public static partial class ContentHelperExtensions {
    public static BlockListModel GetBlockList(this IContentHelper contentHelper, ElementsProperty property) {
        if (!property.Type.IsBlockList()) {
            throw new Exception("Property is not block list content");
        }
        
        return GetBlockList(contentHelper, property.ContentType.Alias, property.Type.Alias, property.Json);
    }
    
    public static BlockListModel GetBlockList(this IContentHelper contentHelper,
                                              string contentTypeAlias,
                                              IProperty property) {
        if (!property.PropertyType.IsBlockList()) {
            throw new Exception("Property is not block list content");
        }
        
        return GetBlockList(contentHelper, contentTypeAlias, property.PropertyType.Alias, property.GetValue());
    }
    
    public static BlockListModel GetBlockList(this IContentHelper contentHelper,
                                              string contentTypeAlias,
                                              string propertyTypeAlias,
                                              object propertyValue) {
        var blockListModel = contentHelper.GetConvertedValue<BlockListPropertyValueConverter, BlockListModel>(contentTypeAlias,
                                                                                                              propertyTypeAlias,
                                                                                                              propertyValue);

        return blockListModel;
    }
}
