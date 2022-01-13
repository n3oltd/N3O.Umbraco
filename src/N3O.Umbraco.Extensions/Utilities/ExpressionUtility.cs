using System;
using System.Linq.Expressions;

namespace N3O.Umbraco.Utilities {
    public static class ExpressionUtility {
        public static MemberExpression ToMemberExpression<TModel, TValue>(Expression<Func<TModel, TValue>> expr) {
            expr = StripConvert(expr);
            
            var memberExpression = expr.Body as MemberExpression;

            if (memberExpression == null) {
                throw new Exception("Expression is not a member expression");
            }

            return memberExpression;
        }

        private static Expression<Func<TModel, TValue>> StripConvert<TModel, TValue>(Expression<Func<TModel, TValue>> source) {
            var result = source.Body;
            
            while ((result.NodeType == ExpressionType.Convert || result.NodeType == ExpressionType.ConvertChecked) &&
                   result.Type == typeof(TValue)) {
                result = ((UnaryExpression) result).Operand;
            }
            
            var memberName = ((MemberExpression) result).Member.Name;
            var parameter = Expression.Parameter(typeof(TModel));
            var property = Expression.Property(parameter, memberName);
            return Expression.Lambda<Func<TModel, TValue>>(property, parameter);
        }
    }
}
