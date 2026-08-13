using N3O.Umbraco.Extensions;
using N3O.Umbraco.Search.Typesense.Attributes;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace N3O.Umbraco.Search.Typesense;

public static class TypesenseField {
    private static readonly NamingStrategy CamelCase = new CamelCaseNamingStrategy {
        ProcessDictionaryKeys = false
    };

    public static string Get<T, TField>(Expression<Func<T, TField>> pathExpression) {
        var pathComponents = new List<string>();
        MemberExpression memberExpression;

        switch (pathExpression.Body.NodeType) {
            case ExpressionType.Convert:
            case ExpressionType.ConvertChecked:
                memberExpression = (pathExpression.Body as UnaryExpression)?.Operand as MemberExpression;
                break;

            default:
                memberExpression = pathExpression.Body as MemberExpression;
                break;
        }

        while (memberExpression != null) {
            var propertyName = ToSearchFieldName(memberExpression.Member);

            pathComponents.Insert(0, propertyName);

            memberExpression = memberExpression.Expression as MemberExpression;
        }

        return string.Join(PathSeparator, pathComponents);
    }

    public static string ToSearchFieldName(MemberInfo memberInfo) {
        var propertyInfo = memberInfo as PropertyInfo;

        var fieldAttribute = propertyInfo?.GetCustomAttribute<FieldAttribute>();
        if (fieldAttribute != null) {
            return fieldAttribute.Name;
        }

        var jsonProperty = propertyInfo?.GetCustomAttribute<JsonPropertyAttribute>();
        if (jsonProperty?.PropertyName.HasValue() == true) {
            return jsonProperty.PropertyName;
        }

        return CamelCase.GetPropertyName(memberInfo.Name, hasSpecifiedName: false);
    }

    public const char PathSeparator = '.';
}
