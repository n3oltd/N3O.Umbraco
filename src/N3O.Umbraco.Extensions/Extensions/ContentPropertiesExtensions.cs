using N3O.Umbraco.Content;
using System.Linq;

namespace N3O.Umbraco.Extensions;

public static class ContentPropertiesExtensions {
    public static ElementsProperty GetElementsPropertyByAlias(this ContentProperties contentProperties, string alias) {
        var elementsProperty = contentProperties.ElementsProperties
                                                .SingleOrDefault(x => x.Alias.EqualsInvariant(alias));

        return elementsProperty;
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
    
    public static T GetPropertyValueByAlias<T>(this ContentProperties contentProperties, string alias) {
        return (T) GetPropertyByAlias(contentProperties, alias)?.Value;
    }
}
