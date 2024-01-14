using FluentValidation;
using N3O.Umbraco.Giving.Models;
using N3O.Umbraco.Json;

namespace N3O.Umbraco.Crowdfunding.Models;


// We would use a composer to scan assemblies for all IAllocationValidator implementations and we would register
// these as the interface. We can then inject an IEnumerable<IAllocationValidator> into the AllocationReqValidator
// constructor, and we can then loop through and call validate on these to ensure the validation all happens before
// the request proceeds.

public class CrowdfundingAllocationExtensionValidator : AllocationExtensionValidator<CrowdfundingDataReq> {
    public CrowdfundingAllocationExtensionValidator(IJsonProvider jsonProvider, IValidator<CrowdfundingDataReq> validator)
        : base(jsonProvider, validator) { }
    
    public override bool CanValidate(AllocationReq allocationReq) {
        return allocationReq.Extensions.ContainsKey(Key);
    }
    
    protected override string Key => CrowdfundingConstants.CrowdfundingAllocation.Key;
}