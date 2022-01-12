using N3O.Umbraco.Constants;
using Umbraco.Cms.Core.Models;

namespace N3O.Umbraco.Extensions {
    public static class PropertyTypeExtensions {
        public static bool HasEditorAlias(this IPropertyType propertyType, string alias) {
            return propertyType.PropertyEditorAlias.EqualsInvariant(alias);
        }
        
        public static bool IsBlockContent(this IPropertyType propertyType) {
            return propertyType.HasEditorAlias(Perplex.ContentBlocks.Constants.PropertyEditor.Alias);
        }
    
        public static bool IsNestedContent(this IPropertyType propertyType) {
            return propertyType.HasEditorAlias(PropertyEditors.Aliases.NestedContent);
        }
    }
}
