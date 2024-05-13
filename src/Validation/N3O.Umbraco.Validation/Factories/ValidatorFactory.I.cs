using FluentValidation;
using System;
using System.Collections.Generic;

namespace N3O.Umbraco.Validation;

public interface IValidatorFactory {
    IEnumerable<IValidator> CreateValidatorsIfDefined(Type modelType);
}