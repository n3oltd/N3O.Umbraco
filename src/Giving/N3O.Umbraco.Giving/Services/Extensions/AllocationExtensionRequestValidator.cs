using FluentValidation;
using FluentValidation.Results;
using N3O.Umbraco.Giving.Extensions;
using N3O.Umbraco.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace N3O.Umbraco.Giving;

public abstract class AllocationExtensionRequestValidator<TReq> : IAllocationExtensionRequestValidator {
    private readonly IJsonProvider _jsonProvider;
    private readonly IValidator<TReq> _validator;

    protected AllocationExtensionRequestValidator(IJsonProvider jsonProvider, IValidator<TReq> validator) {
        _jsonProvider = jsonProvider;
        _validator = validator;
    }

    public ValidationResult Validate(IDictionary<string, JToken> req) {
        var model = req.Get<TReq>(_jsonProvider, Key);

        return _validator.Validate(model);
    }

    public abstract string Key { get; }
}