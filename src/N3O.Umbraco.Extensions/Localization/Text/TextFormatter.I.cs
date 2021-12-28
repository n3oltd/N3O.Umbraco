using N3O.Umbraco.Lookups;
using System;

namespace N3O.Umbraco.Localization {
    public interface ITextFormatter {
        string Format<TStrings>(Func<TStrings, string> propertySelector, params object[] formatArgs)
            where TStrings : class, IStrings, new();

        string FormatLookupName(INamedLookup lookup);
    }
}
