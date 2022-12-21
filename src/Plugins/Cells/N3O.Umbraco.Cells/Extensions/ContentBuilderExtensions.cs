using N3O.Umbraco.Content;
using N3O.Umbraco.Cells.Content;

namespace N3O.Umbraco.Cells.Extensions;

public static class ContentBuilderExtensions {
    public static CellsPropertyBuilder Cells(this IContentBuilder builder, string propertyTypeAlias) {
        return builder.Property<CellsPropertyBuilder>(propertyTypeAlias);
    }
}
