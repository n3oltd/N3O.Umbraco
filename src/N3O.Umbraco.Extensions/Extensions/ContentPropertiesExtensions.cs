using N3O.Umbraco.Content;
using System.Linq;

namespace N3O.Umbraco.Extensions;

public static class ContentPropertiesExtensions {
    public static NestedContentProperty GetNestedContentPropertyByAlias(this ContentProperties contentProperties, string alias) {
        var nestedContentProperty = contentProperties.NestedContentProperties
                                                     .SingleOrDefault(x => x.Alias.EqualsInvariant(alias));

        return nestedContentProperty;
    }
    
    public static IContentProperty GetPropertyByAlias(this ContentProperties contentProperties, string alias) {
        IContentProperty contentProperty = contentProperties.Properties
                                                            .SingleOrDefault(x => x.Alias.EqualsInvariant(alias));

        if (contentProperty == null) {
            contentProperty = contentProperties.NestedContentProperties
                                               .SingleOrDefault(x => x.Alias.EqualsInvariant(alias));
        }

        return contentProperty;
    }
    
    public static T GetPropertyValueByAlias<T>(this ContentProperties contentProperties, string alias) {
        return (T) GetPropertyByAlias(contentProperties, alias)?.Value;
    }
}
