using N3O.Umbraco.Content;
using N3O.Umbraco.SerpEditor.Content;

namespace N3O.Umbraco.SerpEditor.Extensions;

public static class ContentBuilderExtensions {
    public static SerpEditorPropertyBuilder SerpEditor(this IContentBuilder builder, string propertyTypeAlias) {
        return builder.Property<SerpEditorPropertyBuilder>(propertyTypeAlias);
    }
}
