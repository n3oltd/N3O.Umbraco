using N3O.Umbraco.ContentTypes;
using N3O.Umbraco.Cropper.ContentTypes;
using System;
using System.Linq.Expressions;

namespace N3O.Umbraco.Cropper.Extensions;

public static class PropertyContainerBuilderExtensions {
    public static CropperPropertyTypeBuilder Cropper(this IPropertyContainerBuilder builder, string propertyAlias) {
        return builder.Property<CropperPropertyTypeBuilder>(propertyAlias);
    }

    public static CropperPropertyTypeBuilder Cropper<T, TProperty>(this IPropertyContainerBuilder<T> builder,
                                                                   Expression<Func<T, TProperty>> expression) {
        return builder.Property<CropperPropertyTypeBuilder, TProperty>(expression);
    }
}
