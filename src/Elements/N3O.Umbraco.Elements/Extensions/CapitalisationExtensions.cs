using N3O.Umbraco.Elements.Clients;
using N3O.Umbraco.Exceptions;
using N3O.Umbraco.Lookups;

namespace N3O.Umbraco.Elements.Extensions;

public static class CapitalisationExtensions {
    public static TextTransformation ToTextTransformation(this Capitalisation capitilisation) {
        if (capitilisation == Capitalisations.Upper) {
            return TextTransformation.Uppercase;
        } else if (capitilisation == Capitalisations.Lower) {
            return TextTransformation.Lowercase;
        } else if (capitilisation == Capitalisations.Title) {
            return TextTransformation.Titlecase;
        } else {
            throw UnrecognisedValueException.For(capitilisation);
        }
    }
}