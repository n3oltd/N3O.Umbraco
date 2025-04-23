using N3O.Umbraco.Content;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace N3O.Umbraco.Extensions;

public static class ContentPropertiesExtensions {
    public static ElementsProperty GetElementsPropertyByAlias(this ContentProperties contentProperties, string alias) {
        var elementsProperty = contentProperties.ElementsProperties
                                                .SingleOrDefault(x => x.Alias.EqualsInvariant(alias));

        return elementsProperty;
    }
    
    public static ElementsProperty GetElementsPropertyByAlias<TContent>(this ContentProperties contentProperties,
                                                                        Expression<Func<TContent, object>> propertyExpression) {
        var alias = AliasHelper<TContent>.PropertyAlias(propertyExpression);

        return GetElementsPropertyByAlias(contentProperties, alias);
    }
    
    public static IContentProperty GetPropertyByAlias(this ContentProperties contentProperties, string alias) {
        IContentProperty contentProperty = contentProperties.Properties
                                                            .SingleOrDefault(x => x.Alias.EqualsInvariant(alias));

        if (contentProperty == null) {
            contentProperty = contentProperties.ElementsProperties
                                               .SingleOrDefault(x => x.Alias.EqualsInvariant(alias));
        }

        return contentProperty;
    }
    
    public static IContentProperty GetPropertyByAlias<TContent>(this ContentProperties contentProperties,
                                                                Expression<Func<TContent, object>> propertyExpression) {
        var alias = AliasHelper<TContent>.PropertyAlias(propertyExpression);

        return GetPropertyByAlias(contentProperties, alias);
    }
    
    public static T GetPropertyValueByAlias<T>(this ContentProperties contentProperties, string alias) {
        return (T) GetPropertyByAlias(contentProperties, alias)?.Value;
    }
    
    public static TValue GetPropertyValueByAlias<TContent, TValue>(this ContentProperties contentProperties,
                                                                   Expression<Func<TContent, object>> propertyExpression) {
        var alias = AliasHelper<TContent>.PropertyAlias(propertyExpression);
        
        return GetPropertyValueByAlias<TValue>(contentProperties, alias);
    }
}
