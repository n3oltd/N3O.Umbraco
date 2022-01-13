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

namespace N3O.Umbraco.Content {
    public static class AliasHelper {
        private static readonly ConcurrentDictionary<Type, string> ContentTypeAliasCache = new();

        public static string ContentTypeAlias(Type contentType) {
            return ContentTypeAliasCache.GetOrAdd(contentType, () => {
                var alias = contentType.GetCustomAttribute<PublishedModelAttribute>()?.ContentTypeAlias ??
                            contentType.GetCustomAttribute<UmbracoContentAttribute>()?.ContentTypeAlias ??
                            contentType.Name.Camelize();

                return alias;
            });
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
}
