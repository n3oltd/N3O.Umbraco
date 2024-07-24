using N3O.Umbraco.Content;
using N3O.Umbraco.Cropper.Models;
using System;
using System.Linq.Expressions;
using Umbraco.Extensions;

namespace N3O.Umbraco.Uploader.Extensions;

public static class UmbracoContentExtensions {
    public static CroppedImage GetCroppedImageAs<T, TProperty>(this UmbracoContent<T> umbracoContent,
                                                               Expression<Func<T, TProperty>> memberExpression) {
        var alias = AliasHelper<T>.PropertyAlias(memberExpression);
        var value = (CroppedImage) umbracoContent.Content().Value(alias);

        return value;
    }
}