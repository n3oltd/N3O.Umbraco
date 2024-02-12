using FluentValidation;
using N3O.Umbraco.Giving.Models;
using N3O.Umbraco.Json;

namespace N3O.Umbraco.Crowdfunding.Models;

public class CrowdfundingAllocationExtensionValidator : AllocationExtensionValidator<CrowdfundingDataReq> {
    public CrowdfundingAllocationExtensionValidator(IJsonProvider jsonProvider, IValidator<CrowdfundingDataReq> validator)
        : base(jsonProvider, validator) { }
    
    public override bool CanValidate(AllocationReq allocationReq) {
        return allocationReq.Extensions.ContainsKey(Key);
    }
    
    protected override string Key => CrowdfundingConstants.CrowdfundingAllocation.Key;
}