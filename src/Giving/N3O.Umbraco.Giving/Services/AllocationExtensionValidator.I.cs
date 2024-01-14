using FluentValidation.Results;

namespace N3O.Umbraco.Giving.Models;

public interface IAllocationExtensionValidator {
    bool CanValidate(AllocationReq allocationReq);
    ValidationResult Validate(AllocationReq allocationReq);
}