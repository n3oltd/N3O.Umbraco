using Humanizer;
using N3O.Umbraco.Lookups;

namespace N3O.Umbraco.Extensions;

public static class LookupExtensions {
    public static TEnum? ToEnum<TEnum>(this ILookup lookup) where TEnum : struct {
        return lookup?.Id.ToEnum<TEnum>() ?? (lookup as INamedLookup)?.Name.ToEnum<TEnum>();
    }
    
    public static string ToViewName(this ILookup lookup) {
        return lookup.Id.Pascalize();
    }
}
