using N3O.Umbraco.Accounts.Models;
using N3O.Umbraco.Localization;

namespace N3O.Umbraco.Accounts.Extensions;

public static class TextFormatterExtensions {
    public static string ToDisplayName(this ITextFormatter textFormatter, IName name) {
        return textFormatter.FormatName(name.Title, name.FirstName, name.LastName);
    }
}