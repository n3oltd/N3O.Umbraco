using N3O.Umbraco.Extensions;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace N3O.Umbraco.Utilities {
    public static class HtmlField {
        public static string Name<TModel>(Expression<Func<TModel, object>> expr) {
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
                var propertyName = memberExpression.Member.Name;
                var propertyType = memberExpression.Type;

                nameComponents.Insert(0, propertyName.Substring(0, 1).ToLowerInvariant() + propertyName.Substring(1));

                memberExpression = memberExpression.Expression as MemberExpression;
            }

            return nameComponents.Join(".");
        }
    }
}
