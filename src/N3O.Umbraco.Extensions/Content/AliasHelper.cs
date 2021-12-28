using Humanizer;
using N3O.Umbraco.Attributes;
using N3O.Umbraco.Extensions;
using System;
using System.Collections.Concurrent;
using System.Linq.Expressions;
using System.Reflection;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Infrastructure.ModelsBuilder;

namespace N3O.Umbraco.Content {
    public static class AliasHelper {
        private static readonly ConcurrentDictionary<Type, string> ContentTypeAliasCache = new();
        private static readonly ConcurrentDictionary<(Type, string), string> PropertyAliasCache = new();

        public static string ForContentType<TContent>() {
            return ForContentType(typeof(TContent));
        }
    
        public static string ForContentType(Type contentType) {
            return ContentTypeAliasCache.GetOrAdd(contentType, () => {
                var alias = contentType.GetCustomAttribute<PublishedModelAttribute>()?.ContentTypeAlias ??
                            contentType.GetCustomAttribute<UmbracoContentAttribute>()?.ContentTypeAlias ??
                            contentType.Name.Pascalize();

                return alias;
            });
        }

        public static string ForProperty<TContent, TProperty>(Expression<Func<TContent, TProperty>> memberExpression) {
            return PropertyAliasCache.GetOrAdd((typeof(TContent), memberExpression.ToString()), _ => {
                var memberInfo = typeof(TContent).GetMember((memberExpression.Body as MemberExpression).Member.Name)[0];
                var alias = memberInfo.GetCustomAttribute<ImplementPropertyTypeAttribute>()?.Alias ??
                            memberInfo.GetCustomAttribute<UmbracoPropertyAttribute>()?.PropertyAlias ??
                            memberInfo.Name.Camelize();

                return alias;
            });
        }
    
        public static string ForProperty(PropertyInfo propertyInfo) {
            return PropertyAliasCache.GetOrAdd((propertyInfo.DeclaringType, propertyInfo.Name), _ => {
                var alias = propertyInfo.GetCustomAttribute<ImplementPropertyTypeAttribute>()?.Alias ??
                            propertyInfo.GetCustomAttribute<UmbracoPropertyAttribute>()?.PropertyAlias ??
                            propertyInfo.Name.Camelize();

                return alias;
            });
        }
    }
}
