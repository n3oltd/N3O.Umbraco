using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Extensions;

public static class PublishedPropertyTypeExtensions {
    public static bool HasEditorAlias(this IPublishedPropertyType propertyType, string alias) {
        return propertyType.EditorAlias.EqualsInvariant(alias);
    }
}
