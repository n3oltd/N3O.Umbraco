using Microsoft.AspNetCore.Html;

namespace N3O.Umbraco.Extensions;

public static class HtmlStringExtensions {
    public static bool HasValue(this HtmlString s) {
        return s?.ToString().HasValue() ?? false;
    }
}
