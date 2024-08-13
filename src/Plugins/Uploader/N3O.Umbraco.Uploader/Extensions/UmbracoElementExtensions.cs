using N3O.Umbraco.Content;
using N3O.Umbraco.Uploader.Models;
using System;
using System.Linq.Expressions;
using Umbraco.Extensions;

namespace N3O.Umbraco.Uploader.Extensions;

public static class UmbracoElementExtensions {
    public static FileUpload GetFileUploadAs<T, TProperty>(this UmbracoElement<T> umbracoElement,
                                                           Expression<Func<T, TProperty>> memberExpression) {
        var alias = AliasHelper<T>.PropertyAlias(memberExpression);
        var value = (FileUpload) umbracoElement.Content().Value(alias);

        return value;
    }
}