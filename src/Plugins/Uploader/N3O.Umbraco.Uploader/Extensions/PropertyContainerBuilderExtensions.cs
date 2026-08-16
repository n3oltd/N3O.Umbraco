using N3O.Umbraco.ContentTypes;
using N3O.Umbraco.Uploader.ContentTypes;
using System;
using System.Linq.Expressions;

namespace N3O.Umbraco.Uploader.Extensions;

public static class PropertyContainerBuilderExtensions {
    public static UploaderPropertyTypeBuilder Uploader(this IPropertyContainerBuilder builder, string propertyAlias) {
        return builder.Property<UploaderPropertyTypeBuilder>(propertyAlias);
    }

    public static UploaderPropertyTypeBuilder Uploader<T, TProperty>(this IPropertyContainerBuilder<T> builder,
                                                                     Expression<Func<T, TProperty>> expression) {
        return builder.Property<UploaderPropertyTypeBuilder, TProperty>(expression);
    }
}
