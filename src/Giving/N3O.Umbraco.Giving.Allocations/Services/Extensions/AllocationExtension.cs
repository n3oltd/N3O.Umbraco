using FluentValidation;
using FluentValidation.Results;
using N3O.Umbraco.Giving.Allocations.Extensions;
using N3O.Umbraco.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace N3O.Umbraco.Giving.Allocations;

public abstract class AllocationExtension<TReq, TModel> :
    IAllocationExtensionRequestBinder, IAllocationExtensionRequestValidator {
    private readonly IJsonProvider _jsonProvider;
    private readonly IValidator<TReq> _validator;

    protected AllocationExtension(IJsonProvider jsonProvider, IValidator<TReq> validator) {
        _jsonProvider = jsonProvider;
        _validator = validator;
    }
    
    public void Bind(IDictionary<string, JToken> src, IDictionary<string, JToken> dest) {
        var req = src.Get<TReq>(_jsonProvider, Key);

        var model = Bind(req);
        
        dest.Set(_jsonProvider, Key, model);
    }
    
    public ValidationResult Validate(IDictionary<string, JToken> req) {
        var model = req.Get<TReq>(_jsonProvider, Key);

        return _validator.Validate(model);
    }

    protected abstract TModel Bind(TReq req);

    public abstract string Key { get; }
}