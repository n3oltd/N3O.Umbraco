using Humanizer;
using N3O.Umbraco.Lookups;

namespace N3O.Umbraco.Extensions;

public static class LookupExtensions {
    public static string ToViewName(this ILookup lookup) {
        return lookup.Id.Pascalize();
    }
}
