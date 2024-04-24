using N3O.Umbraco.Templates;

namespace N3O.Umbraco.Extensions;

public static class StyleContextExtensions {
    public static bool Has(this IStyleContext styleContext, string category, string name) {
        return styleContext.Has(TemplateStyleId.Generate(category, name));
    }
    
    public static bool IsActive(this IStyleContext styleContext, string category, string name) {
        return styleContext.Get(category)?.Name.EqualsInvariant(name) ?? false;
    }
    
    public static int? GetInt(this IStyleContext styleContext, string category, string propertyAlias) {
        return styleContext.Get(category)?.GetProperty<int?>(propertyAlias);
    }
    
    public static string GetString(this IStyleContext styleContext, string category, string propertyAlias) {
        return styleContext.Get(category)?.GetProperty<string>(propertyAlias);
    }
    
    public static object GetValue(this IStyleContext styleContext, string category, string propertyAlias) {
        return styleContext.Get(category)?.GetProperty(propertyAlias);
    }
}