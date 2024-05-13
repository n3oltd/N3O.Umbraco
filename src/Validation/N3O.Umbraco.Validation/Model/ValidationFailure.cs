using N3O.Umbraco.Localization;
using System;

namespace N3O.Umbraco.Validation;

public class ValidationFailure {
    public ValidationFailure(string property, string error) {
        Property = property;
        Error = error;
    }
    
    public static ValidationFailure WithMessage<TStrings>(IFormatter formatter,
                                                          string property,
                                                          Func<TStrings, string> propertySelector,
                                                          params object[] formatArgs)
        where TStrings : class, IStrings, new() {
        return WithMessage(property, formatter.Text.Format(propertySelector, formatArgs));
    }

    public static ValidationFailure WithMessage(string property, string error) {
        var failure = new ValidationFailure(property, error);

        return failure;
    }

    public string Property { get; }
    public string Error { get; }
}