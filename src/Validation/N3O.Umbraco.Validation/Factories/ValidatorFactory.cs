using FluentValidation;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Utilities;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace N3O.Umbraco.Validation;

public class ValidatorFactory : IValidatorFactory {
    private readonly IReadOnlyList<IValidator> _validators;

    public ValidatorFactory(IEnumerable<IValidator> validators) {
        _validators = validators.ToList();
    }

    public IEnumerable<IValidator> CreateValidatorsIfDefined(Type modelType) {
        var validatorTypes = GetValidatorTypesForModel(modelType);

        if (validatorTypes.None()) {
            return Enumerable.Empty<IValidator>();
        }

        var validators = validatorTypes.Select(ConstructValidator).ToList();

        return validators;
    }

    private IValidator ConstructValidator(Type validatorType) {
        var validator = _validators.Single(x => x.GetType() == validatorType);

        return validator;
    }

    private List<Type> GetValidatorTypesForModel(Type modelType) {
        var key = nameof(ValidatorFactory) + nameof(GetValidatorTypesForModel) + modelType.FullName;

        return TypesCache.GetOrAdd(key, _ => {
            var list = new List<Type>();
            var type = modelType;
            
            while (type != null && type != typeof(object)) {
               list.AddRangeIfNotExists(GetValidatorTypes(type));
               
               type = type.BaseType;
            }

            // Gives us the most base validators first
            list.Reverse();

            return list;
        });
    }

    private IEnumerable<Type> GetValidatorTypes(Type modelType) {
        if (modelType == null) {
            return Enumerable.Empty<Type>();
        }

        var validatorTypes = AllValidatorTypes
                            .Select(validatorType => GetMatchingValidatorsForModel(validatorType, modelType))
                            .ExceptNull()
                            .ToList();

        return validatorTypes;
    }

    private Type GetMatchingValidatorsForModel(Type validatorType, Type modelType) {
        var abstractGenericValidatorType = typeof(ModelValidator<>);
        var validatesModelsOfType = validatorType
                                   .GetGenericParameterTypesForInheritedGenericClass(abstractGenericValidatorType)
                                   .Single();

        if (validatesModelsOfType.IsGenericTypeDefinition) {
            var type = modelType;
            
            while (type != null) {
                if (type.IsConstructedGenericType &&
                    type.GetGenericTypeDefinition() == validatesModelsOfType.GetGenericTypeDefinition()) {
                    return validatorType.MakeGenericType(type.GenericTypeArguments);
                }

                type = type.BaseType;
            }
        } else {
            if (validatesModelsOfType.IsAssignableFrom(modelType)) {
                return validatorType;
            }
        }

        return null;
    }

    private static readonly List<Type> AllValidatorTypes =
        OurAssemblies.GetTypes(t => t.IsConcreteClass() &&
                                    t.InheritsGenericClass(typeof(ModelValidator<>)))
                     .ToList();

    private static readonly ConcurrentDictionary<string, List<Type>> TypesCache = new();
}