using N3O.Umbraco.Content;
using N3O.Umbraco.Cropper.Models;
using System;
using System.Linq.Expressions;
using Umbraco.Extensions;

namespace N3O.Umbraco.Uploader.Extensions;

public static class UmbracoElementExtensions {
    public static CroppedImage GetCroppedImageAs<T, TProperty>(this UmbracoElement<T> umbracoElement,
                                                               Expression<Func<T, TProperty>> memberExpression) {
        var alias = AliasHelper<T>.PropertyAlias(memberExpression);
        var value = (CroppedImage) umbracoElement.Content().Value(alias);

        return value;
    }
}