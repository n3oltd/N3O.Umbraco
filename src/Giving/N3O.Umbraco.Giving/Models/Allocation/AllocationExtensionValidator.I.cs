using FluentValidation.Results;

namespace N3O.Umbraco.Giving.Models;

public interface IAllocationExtensionValidator {
    ValidationResult Validate(AllocationReq allocationReq);
}