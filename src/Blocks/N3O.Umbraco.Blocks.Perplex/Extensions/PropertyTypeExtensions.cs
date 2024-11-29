using N3O.Umbraco.Extensions;
using Umbraco.Cms.Core.Models;

namespace N3O.Umbraco.Blocks.Perplex.Extensions;

public static class PropertyTypeExtensions {
    public static bool IsPerplexBlocks(this IPropertyType propertyType) {
        return propertyType.HasEditorAlias(global::Perplex.ContentBlocks.Constants.PropertyEditor.Alias);
    }
}