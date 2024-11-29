using N3O.Umbraco.Content;

namespace N3O.Umbraco.Blocks.Perplex.Extensions;

public static class ContentBuilderExtensions {
    public static ContentBlocksPropertyBuilder PerplexBlocks(this IContentBuilder builder, string propertyTypeAlias) {
        return builder.Property<ContentBlocksPropertyBuilder>(propertyTypeAlias);
    }
}
