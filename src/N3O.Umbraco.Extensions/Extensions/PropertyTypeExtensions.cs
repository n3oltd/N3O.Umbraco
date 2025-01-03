using Umbraco.Cms.Core.Models;
using static Umbraco.Cms.Core.Constants.PropertyEditors;

namespace N3O.Umbraco.Extensions;

public static class PropertyTypeExtensions {
    public static bool HasEditorAlias(this IPropertyType propertyType, string alias) {
        return propertyType.PropertyEditorAlias.EqualsInvariant(alias);
    }

    public static bool IsBlockGrid(this IPropertyType propertyType) {
        return propertyType.HasEditorAlias(Aliases.BlockGrid);
    }
    
    public static bool IsBlockList(this IPropertyType propertyType) {
        return propertyType.HasEditorAlias(Aliases.BlockList);
    }
    
    public static bool IsDataList(this IPropertyType propertyType) {
        return propertyType.HasEditorAlias("Umbraco.Community.Contentment.DataList");
    }
    
    public static bool IsDataPicker(this IPropertyType propertyType) {
        return propertyType.HasEditorAlias("Umbraco.Community.Contentment.DataPicker");
    }
    
    public static bool IsDropdown(this IPropertyType propertyType) {
        return propertyType.HasEditorAlias(Aliases.DropDownListFlexible);
    }
    
    public static bool IsMediaPicker(this IPropertyType propertyType) {
        return propertyType.HasEditorAlias(Aliases.MediaPicker3);
    }
    
    public static bool IsMultiNodeTreePicker(this IPropertyType propertyType) {
        return propertyType.HasEditorAlias(Aliases.MultiNodeTreePicker);
    }

    public static bool IsNestedContent(this IPropertyType propertyType) {
        return propertyType.HasEditorAlias(Aliases.NestedContent);
    }
    
    public static bool IsPerplexBlocks(this IPropertyType propertyType) {
        return propertyType.HasEditorAlias("Perplex.ContentBlocks");
    }
}
