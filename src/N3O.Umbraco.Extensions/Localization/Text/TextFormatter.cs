using N3O.Umbraco.Extensions;
using N3O.Umbraco.Lookups;
using System;
using System.Collections.Concurrent;

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

        //text = _stringLocalizer.Get(strings.Folder, strings.Name, text);
    
        return string.Format(text, formatArgs);
    }

    public string FormatLookupName(INamedLookup lookup) {
        return lookup.Name;
    }

    public string FormatName(string firstName, string lastName) {
        return FormatName(null, firstName, lastName);
    }

    // TODO Implement this properly by loading preference from subscription
    // or similar means as French has SURNAME FirstName for example
    public string FormatName(string title, string firstName, string lastName) {
        var names = new[] {
            title, firstName, lastName
        }.ExceptNull();

        var formattedName = string.Join(" ", names);

        return formattedName;
    }

    public static ITextFormatter Default = new TextFormatter(new InvariantStringLocalizer());

    public static ITextFormatter Create(IStringLocalizer stringLocalizer) {
        return new TextFormatter(stringLocalizer);
    }
}
