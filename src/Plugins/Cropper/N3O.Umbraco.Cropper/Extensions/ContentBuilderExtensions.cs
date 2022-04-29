using N3O.Umbraco.Content;
using N3O.Umbraco.Cropper.Content;

namespace N3O.Umbraco.Cropper.Extensions {
    public static class ContentBuilderExtensions {
        public static CropperPropertyBuilder Cropper(this IContentBuilder builder, string propertyTypeAlias) {
            return builder.Property<CropperPropertyBuilder>(propertyTypeAlias);
        }
    }
}