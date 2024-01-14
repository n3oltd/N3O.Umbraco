using FluentValidation;
using FluentValidation.Results;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Json;

namespace N3O.Umbraco.Giving.Models;

public abstract class AllocationExtensionValidator<TReq> : IAllocationExtensionValidator {
    private readonly IJsonProvider _jsonProvider;
    private readonly IValidator<TReq> _validator;

    protected AllocationExtensionValidator(IJsonProvider jsonProvider, IValidator<TReq> validator) {
        _jsonProvider = jsonProvider;
        _validator = validator;
    }

    public ValidationResult Validate(AllocationReq allocationReq) {
        if (allocationReq.Extensions.HasValue() && allocationReq.Extensions.ContainsKey(Key)) {
            var extensionDataReq = _jsonProvider.DeserializeObject<TReq>(allocationReq.Extensions[Key].ToString());

            return _validator.Validate(extensionDataReq);
        } else {
            return new ValidationResult();   
        }
    }
    
    protected abstract string Key { get; }

    public abstract bool CanValidate(AllocationReq allocationReq);
}