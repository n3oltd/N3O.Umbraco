﻿using N3O.Umbraco.Content;
using N3O.Umbraco.Uploader.Content;

namespace N3O.Umbraco.Uploader.Extensions {
    public static class ContentBuilderExtensions {
        public static UploaderPropertyBuilder Uploader(this IContentBuilder builder, string propertyTypeAlias) {
            return builder.Property<UploaderPropertyBuilder>(propertyTypeAlias);
        }
    }
}