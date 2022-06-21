using Humanizer;
using N3O.Umbraco.Attributes;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Utilities;
using System;
using System.Collections.Concurrent;
using System.Linq.Expressions;
using System.Reflection;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Infrastructure.ModelsBuilder;

namespace N3O.Umbraco.Content;

public static class AliasHelper {
    private static readonly ConcurrentDictionary<Type, string> ContentTypeAliasCache = new();

    public static string ContentTypeAlias(Type type) {
        return ContentTypeAliasCache.GetOrAdd(type, () => {
            var alias = type.GetCustomAttribute<PublishedModelAttribute>()?.ContentTypeAlias ??
                        type.GetCustomAttribute<UmbracoContentAttribute>()?.ContentTypeAlias ??
                        GetContentTypeAlias(type);

            return alias;
        });
    }

    private static string GetContentTypeAlias(Type type) {
        var contentTypeAlias = type.Name.Camelize();

        if (type.IsSubclassOrSubInterfaceOfGenericType(typeof(UmbracoContent<>)) &&
            contentTypeAlias.EndsWith("Content")) {
            contentTypeAlias = contentTypeAlias.Substring(0, contentTypeAlias.Length - "Content".Length);
        }
        
        if (type.IsSubclassOrSubInterfaceOfGenericType(typeof(UmbracoElement<>)) &&
            contentTypeAlias.EndsWith("Element")) {
            contentTypeAlias = contentTypeAlias.Substring(0, contentTypeAlias.Length - "Element".Length);
        }


        return contentTypeAlias;
    }
}

public static class AliasHelper<TContent> {
    private static readonly ConcurrentDictionary<(Type, string), string> PropertyAliasCache = new();

    public static string ContentTypeAlias() {
        return AliasHelper.ContentTypeAlias(typeof(TContent));
    }

    public static string PropertyAlias<TProperty>(Expression<Func<TContent, TProperty>> expr) {
        return PropertyAliasCache.GetOrAdd((typeof(TContent), expr.ToString()), _ => {
            var memberInfo = ExpressionUtility.ToMemberExpression(expr).Member;

            return PropertyAlias((PropertyInfo) memberInfo);
        });
    }

    public static string PropertyAlias(PropertyInfo propertyInfo) {
        return PropertyAliasCache.GetOrAdd((propertyInfo.DeclaringType, propertyInfo.Name), _ => {
            var alias = propertyInfo.GetCustomAttribute<ImplementPropertyTypeAttribute>()?.Alias ??
                        propertyInfo.GetCustomAttribute<UmbracoPropertyAttribute>()?.PropertyAlias ??
                        propertyInfo.Name.Camelize();

            return alias;
        });
    }
}
