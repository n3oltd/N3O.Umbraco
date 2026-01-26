using Humanizer;

namespace N3O.Umbraco.Templates.Extensions;

public static class StringExtensions {
    public static string ToModelKey(this string s) {
        return s.Kebaberize().Replace('-', '_');
    }
}
