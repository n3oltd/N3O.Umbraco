using N3O.Umbraco.Content;
using N3O.Umbraco.Uploader.Models;
using System;
using System.Linq.Expressions;
using Umbraco.Extensions;

namespace N3O.Umbraco.Uploader.Extensions;

public static class UmbracoContentExtensions {
    public static FileUpload GetFileUploadAs<T, TProperty>(this UmbracoContent<T> umbracoContent,
                                                           Expression<Func<T, TProperty>> memberExpression) {
        var alias = AliasHelper<T>.PropertyAlias(memberExpression);
        var value = (FileUpload) umbracoContent.Content().Value(alias);

        return value;
    }
}