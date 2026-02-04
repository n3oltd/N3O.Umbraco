using N3O.Umbraco.Cells.Content;
using N3O.Umbraco.Content;

namespace N3O.Umbraco.Cells.Extensions;

public static class ContentBuilderExtensions {
    public static CellsPropertyBuilder Cells(this IContentBuilder builder, string propertyAlias) {
        return builder.Property<CellsPropertyBuilder>(propertyAlias);
    }
}
