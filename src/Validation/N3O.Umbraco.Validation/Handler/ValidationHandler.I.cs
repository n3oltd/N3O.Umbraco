using N3O.Umbraco.Localization;
using System;
using System.Collections.Generic;

namespace N3O.Umbraco.Validation;

public interface IValidationHandler {
    void Handle<TStrings>(string property, Func<TStrings, string> propertySelector, params object[] args)
        where TStrings : class, IStrings, new() { }
    
    void Handle<TStrings>(Func<TStrings, string> propertySelector, params object[] args)
        where TStrings : class, IStrings, new() { }
    
    void Handle(ValidationFailure failure) { }
    void Handle(IEnumerable<ValidationFailure> failures) { }
}