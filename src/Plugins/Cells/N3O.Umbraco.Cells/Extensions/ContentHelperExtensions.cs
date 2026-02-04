using N3O.Umbraco.Cells.DataTypes;
using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using System;
using Umbraco.Cms.Core.Models;

namespace N3O.Umbraco.Cells.Extensions;

public static class ContentHelperExtensions {
    public static object[][] GetCells(this IContentHelper contentHelper,
                                           ContentProperties contentProperties,
                                           string propertyTypeAlias) {
        var contentProperty = contentProperties.GetPropertyByAlias(propertyTypeAlias);
        
        return GetCells(contentHelper, contentProperty);
    }
    
    public static object[][] GetCells(this IContentHelper contentHelper, IContentProperty property) {
        if (!property.Type.HasEditorAlias(CellsConstants.PropertyEditorAlias)) {
            throw new Exception("Property is not cells");
        }
        
        return GetCells(contentHelper, property.ContentType.Alias, property.Type.Alias, property.Value);
    }
    
    public static object[][] GetCells(this IContentHelper contentHelper,
                                      string contentTypeAlias,
                                      IProperty property) {
        if (!property.PropertyType.HasEditorAlias(CellsConstants.PropertyEditorAlias)) {
            throw new Exception("Property is not cells");
        }
        
        return GetCells(contentHelper, contentTypeAlias, property.PropertyType.Alias, property.GetValue());
    }

    public static object[][] GetCells(this IContentHelper contentHelper,
                                      string contentTypeAlias,
                                      string propertyTypeAlias,
                                      object propertyValue) {
        if (propertyValue == null) {
            return null;
        }

        var cells = contentHelper.GetConvertedValue<CellsValueConverter, object[][]>(contentTypeAlias,
                                                                                     propertyTypeAlias,
                                                                                     propertyValue);

        return cells;
    }
}
