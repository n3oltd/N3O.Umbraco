using FluentValidation;
using FluentValidation.Results;
using Humanizer;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Validation;

public class Validation : IValidation {
    private readonly IValidatorFactory _validatorFactory;

    public Validation(IValidatorFactory validatorFactory) {
        _validatorFactory = validatorFactory;
    }

    public async Task<IEnumerable<ValidationFailure>> ValidateModelAsync(Type modelType,
                                                                         object model,
                                                                         bool implicitlyValidateChildProperties = false,
                                                                         string validationPrefix = null,
                                                                         CancellationToken cancellationToken = default) {
        var validationFailures = new List<ValidationFailure>();

        validationFailures.AddRange(await GetValidationFailuresAsync(modelType,
                                                                     model,
                                                                     validationPrefix,
                                                                     implicitlyValidateChildProperties,
                                                                     _validatorFactory,
                                                                     cancellationToken));

        return validationFailures;
    }

    public Task<IEnumerable<ValidationFailure>> ValidateModelAsync<T>(T model,
                                                                      bool implicitlyValidateChildProperties = false,
                                                                      string validationPrefix = null,
                                                                      CancellationToken cancellationToken = default) {
        return ValidateModelAsync(typeof(T),
                                  model,
                                  implicitlyValidateChildProperties,
                                  validationPrefix,
                                  cancellationToken);
    }

    private async Task<IEnumerable<ValidationFailure>> GetValidationFailuresAsync(Type modelType,
                                                                                  object model,
                                                                                  string prefix,
                                                                                  bool implicitlyValidateChildProperties,
                                                                                  IValidatorFactory validatorFactory,
                                                                                  CancellationToken cancellationToken) {
        var validationFailures = new List<ValidationFailure>();

        if (model == null || !RequiresValidation(modelType)) {
            return validationFailures;
        }

        var failures1 = await ValidateModelAsync(modelType,
                                                 model,
                                                 prefix,
                                                 validatorFactory,
                                                 cancellationToken);

        validationFailures.AddRange(failures1);

        if (implicitlyValidateChildProperties) {
            var modelProperties = modelType
                                 .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                                 .ExceptNull()
                                 .Where(PropertyRequiresValidation);

            foreach (var property in modelProperties) {
                var propType = property.PropertyType;
                var propValue = property.GetValue(model);
                var propertyNamePrefix = GetPropertyNamePrefix(prefix, property);

                if (propValue is IEnumerable collection) {
                    var elementIndex = 0;
                    foreach (var element in collection) {
                        var elementPrefix = $"{propertyNamePrefix}[{elementIndex}]";

                        var failures2 = await GetValidationFailuresAsync(element?.GetType(),
                                                                         element,
                                                                         elementPrefix,
                                                                         true,
                                                                         validatorFactory,
                                                                         cancellationToken);

                        validationFailures.AddRange(failures2);

                        elementIndex++;
                    }
                } else {
                    var failures3 = await GetValidationFailuresAsync(propType,
                                                                     propValue,
                                                                     propertyNamePrefix,
                                                                     true,
                                                                     validatorFactory,
                                                                     cancellationToken);

                    validationFailures.AddRange(failures3);
                }
            }
        }

        return validationFailures;
    }
    
    private string GetPropertyNamePrefix(string prefix, string propertyName) {
        return prefix.CombineWith(propertyName.Camelize(), ".");
    }

    private string GetPropertyNamePrefix(string prefix, PropertyInfo propertyInfo) {
        if (propertyInfo.HasAttribute<NoPrefixAttribute>()) {
            return "";
        }

        return GetPropertyNamePrefix(prefix, propertyInfo.Name);
    }

    private bool PropertyRequiresValidation(PropertyInfo propertyInfo) {
        if (propertyInfo.IsIndexer()) {
            return false;
        }

        if (propertyInfo.HasAttribute<NoValidationAttribute>()) {
            return false;
        }

        if (propertyInfo.PropertyType?.HasAttribute<NoValidationAttribute>() == true) {
            return false;
        }

        return RequiresValidation(propertyInfo.PropertyType);
    }

    private bool RequiresValidation(Type type) {
        if (type.HasAttribute<NoValidationAttribute>()) {
            return false;
        }

        if (type.IsLookup()) {
            return false;
        }

        if (type.IsGenericType) {
            return true;
        }

        if (OurAssemblies.IsOurAssembly(type.Assembly)) {
            return true;
        }

        return false;
    }
    
    private async Task<List<ValidationFailure>> ValidateModelAsync(Type modelType,
                                                                   object model,
                                                                   string propertyNamePrefix,
                                                                   IValidatorFactory validatorFactory,
                                                                   CancellationToken cancellationToken) {
        var failures = new List<ValidationFailure>();

        if (model != null) {
            var allValidators = validatorFactory.GetAllValidators(modelType).ToList();

            var validationFailures = await RunValidatorsAsync(allValidators,
                                                              modelType,
                                                              model,
                                                              propertyNamePrefix,
                                                              cancellationToken);

            failures.AddRange(validationFailures);
        }

        return failures;
    }

    private async Task<IReadOnlyList<ValidationFailure>> RunValidatorsAsync(IEnumerable<IValidator> validators,
                                                                            Type modelType,
                                                                            object model,
                                                                            string propertyNamePrefix,
                                                                            CancellationToken cancellationToken) {
        var failures = new List<ValidationFailure>();

        foreach (var validator in validators.OrEmpty()) {
            var validationContext = (IValidationContext) Activator.CreateInstance(typeof(ValidationContext<>).MakeGenericType(modelType), model);

            var result = await validator.ValidateAsync(validationContext, cancellationToken);

            failures.AddRange(GetValidationFailures(result, propertyNamePrefix));
        }

        return failures;
    }

    private IEnumerable<ValidationFailure> GetValidationFailures(ValidationResult validationResult, 
                                                                 string propertyNamePrefix) {
        var validationFailures = validationResult
                                .Errors
                                .Select(x => ValidationFailure.WithMessage(GetPropertyNamePrefix(propertyNamePrefix, x.PropertyName), x.ErrorMessage));

        return validationFailures;
    }
}