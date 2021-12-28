using N3O.Umbraco.Extensions;
using N3O.Umbraco.Lookups;
using System;
using System.Collections.Concurrent;
using System.Text.RegularExpressions;

namespace N3O.Umbraco.Localization;

public class TextFormatter : ITextFormatter {
    private readonly IStringLocalizer _stringLocalizer;
    private static readonly ConcurrentDictionary<Type, IStrings> Strings = new();

    public TextFormatter(IStringLocalizer stringLocalizer) {
        _stringLocalizer = stringLocalizer;
    }

    public string Format<TStrings>(Func<TStrings, string> propertySelector, params object[] formatArgs)
        where TStrings : class, IStrings, new() {
        var strings = (TStrings) Strings.GetOrAdd(typeof(TStrings), () => new TStrings());

        var text = propertySelector(strings);

        text = _stringLocalizer.Get(strings.Folder, strings.Name, text);
        
        return string.Format(text, formatArgs);
    }

    public string FormatLookupName(INamedLookup lookup) {
        return lookup.Name;
    }

    public string FormatName(string firstName, string lastName) => FormatName(null, null, firstName, lastName);

    public string FormatName(string title, string firstName, string lastName) => FormatName(null, title, firstName, lastName);

    public string FormatName(string organisation, string title, string firstName, string lastName) {
        // TODO localize and move
        var output = "{Organisation} {CareOf} {Title} {First} {Last}";

        output = CaseSensitivePlaceholderSubstitution(output, "Title", title);
        output = CaseSensitivePlaceholderSubstitution(output, "First", firstName);
        output = CaseSensitivePlaceholderSubstitution(output, "Last", lastName);
        output = CaseSensitivePlaceholderSubstitution(output, "CareOf", ((firstName.HasValue() || lastName.HasValue()) && organisation.HasValue()) ? "C/O" : string.Empty);
        output = CaseSensitivePlaceholderSubstitution(output, "Organisation", organisation);

        return Regex.Replace(Regex.Replace(output, @"( )+", " ").Trim(), "\n+", "\n").Trim();
    }
    
    private static string CaseSensitivePlaceholderSubstitution(string formatString,
                                                               string placeholderName,
                                                               string placeholderValue) {
        var output = formatString;

        output = output.Replace("{" + placeholderName + "}", placeholderValue ?? "");
        output = output.Replace("{" + placeholderName.ToUpperInvariant() + "}", placeholderValue?.ToUpper() ?? "");
        output = output.Replace("{" + placeholderName.ToLowerInvariant() + "}", placeholderValue?.ToLower() ?? "");

        return output;
    }
}
