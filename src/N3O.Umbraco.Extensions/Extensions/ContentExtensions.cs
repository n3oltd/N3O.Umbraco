using N3O.Umbraco.Content;
using System;
using System.Linq.Expressions;
using Umbraco.Cms.Core.Models;

namespace N3O.Umbraco.Extensions {
    public static class ContentExtensions {
        public static TProperty GetValue<TContent, TProperty>(this IContent content,
                                                              Expression<Func<TContent, TProperty>> memberLambda) {
            return GetValue<TContent, TProperty, TProperty>(content, memberLambda);
        }

        public static TValue GetValue<TContent, TProperty, TValue>(this IContent content,
                                                                   Expression<Func<TContent, TProperty>> memberLambda) {
            var propertyAlias = AliasHelper.ForProperty(memberLambda);

            return content.GetValue<TValue>(propertyAlias);
        }
    
        public static bool SetNameIfChanged(this IContent content, string newName) {
            if (content.Name == newName) {
                return false;
            }

            content.Name = newName;

            return true;
        }
    
        public static void SetValue<TContent, TProperty>(this IContent content,
                                                         Expression<Func<TContent, TProperty>> memberLambda,
                                                         TProperty value) {
            content.SetValue(AliasHelper.ForProperty(memberLambda), value);
        }

        public static bool SetValueIfChanged<TContent, TProperty>(this IContent content,
                                                                  Expression<Func<TContent, TProperty>> memberLambda,
                                                                  TProperty value) {
            var changed = false;

            var currentValue = content.GetValue(memberLambda);

            if ((currentValue == null && value != null) ||
                (currentValue != null && value == null) ||
                (currentValue != null && !currentValue.Equals(value))) {
                content.SetValue(memberLambda, value);

                changed = true;
            }

            return changed;
        }
    }
}
