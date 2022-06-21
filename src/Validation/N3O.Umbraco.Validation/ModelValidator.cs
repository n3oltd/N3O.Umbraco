using FluentValidation;
using N3O.Umbraco.Localization;
using System;

namespace N3O.Umbraco.Validation;

public abstract class ModelValidator<T> : AbstractValidator<T> {
    protected ModelValidator(IFormatter formatter) {
        Formatter = formatter;
    }

    protected string Get<TStrings>(Func<TStrings, string> property, params object[] args)
        where TStrings : class, IStrings, new() {
        return Formatter.Text.Format(property, args);
    }

    protected IFormatter Formatter { get; }
}
