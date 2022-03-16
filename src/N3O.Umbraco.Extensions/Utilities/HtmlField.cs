using Humanizer;
using N3O.Umbraco.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace N3O.Umbraco.Utilities {
    public static class HtmlField {
        public static string Name<TModel>(Expression<Func<TModel, object>> expr, int index = -1) {
            return Name<TModel>(expr, null, index);
        }

        public static string Name<TModel>(Expression<Func<TModel, object>> expr, string subExpression, int index = -1) {
            var nameComponents = new List<string>();
            MemberExpression memberExpression;

            switch (expr.Body.NodeType) {
                case ExpressionType.Convert:
                case ExpressionType.ConvertChecked:
                    memberExpression = (expr.Body as UnaryExpression)?.Operand as MemberExpression;
                    break;

                default:
                    memberExpression = expr.Body as MemberExpression;
                    break;
            }

            while (memberExpression != null) {
                var property = (PropertyInfo) memberExpression.Member;
                var name = property.Name.Camelize();
                var isCollection = property.PropertyType.IsCollectionType();
                
                if (isCollection) {
                    if (index == -1) {
                        throw new Exception("Expression contains an enumerable property but no index has been specified");
                    }

                    nameComponents.Insert(0, index.ToString());
                }

                nameComponents.Insert(0, name);

                memberExpression = memberExpression.Expression as MemberExpression;
            }

            var res = nameComponents.Join(".");
            
            if (subExpression.HasValue()) {
                res += $".{subExpression}";
            }

            return res;
        }
    }
}
