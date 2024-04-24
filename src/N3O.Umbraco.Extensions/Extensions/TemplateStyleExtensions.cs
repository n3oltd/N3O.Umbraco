using N3O.Umbraco.Templates;

namespace N3O.Umbraco.Extensions;

public static class TemplateStyleExtensions {
    public static T GetProperty<T>(this ITemplateStyle templateStyle, string propertyAlias) {
        return (T) templateStyle.GetProperty(propertyAlias);
    }
}