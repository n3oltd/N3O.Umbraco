using Umbraco.Cms.Core.Strings;

namespace N3O.Umbraco.Extensions {
    public static class HtmlEncodedStringExtensions {
        public static bool HasValue(this IHtmlEncodedString s) {
            return s?.ToHtmlString().HasValue() ?? false;
        }
    }
}
