using N3O.Umbraco.Templates;

namespace N3O.Umbraco.Extensions;

public static class StyleContextExtensions {
    public static bool Has(this IStyleContext styleContext, string category, string name) {
        return styleContext.Has(TemplateStyleId.Generate(category, name));
    }
    
    public static bool IsActive(this IStyleContext styleContext, string category, string name) {
        return styleContext.Get(category)?.Name.EqualsInvariant(name) ?? false;
    }
    
    public static object GetStyleValue(this IStyleContext styleContext, string category, string propertyAlias) {
        return styleContext.Get(category)?.GetProperty(propertyAlias);
    }
    
    public static int? GetIntStyleValue(this IStyleContext styleContext, string category, string propertyAlias) {
        return styleContext.Get(category)?.GetProperty<int?>(propertyAlias);
    }
    
    public static string GetStringStyleValue(this IStyleContext styleContext, string category, string propertyAlias) {
        return styleContext.Get(category)?.GetProperty<string>(propertyAlias);
    }
}