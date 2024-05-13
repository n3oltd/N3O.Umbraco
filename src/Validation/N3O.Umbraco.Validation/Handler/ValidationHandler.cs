using N3O.Umbraco.Localization;
using System;
using System.Collections.Generic;
using Umbraco.Extensions;

namespace N3O.Umbraco.Validation;

public class ValidationHandler : IValidationHandler {
    private readonly IFormatter _formatter;

    public ValidationHandler(IFormatter formatter) {
        _formatter = formatter;
    }
    
    public void Handle<TStrings>(string property, Func<TStrings, string> propertySelector, params object[] args)
        where TStrings : class, IStrings, new() {
        var failure = ValidationFailure.WithMessage(_formatter,
                                                    property,
                                                    propertySelector,
                                                    args);

        Handle(failure);
    }
    
    public void Handle<TStrings>(Func<TStrings, string> propertySelector, params object[] args)
        where TStrings : class, IStrings, new() {
        Handle(null, propertySelector, args);
    }
    
    public void Handle(ValidationFailure failure) {
        Handle(failure.Yield());
    }

    public void Handle(IEnumerable<ValidationFailure> failures) {
        throw new ValidationException(failures);
    }
}