using N3O.Umbraco.Crowdfunding.Models;
using N3O.Umbraco.Giving.Extensions;
using N3O.Umbraco.Giving.Models;
using N3O.Umbraco.Json;
using static N3O.Umbraco.Crowdfunding.CrowdfundingConstants;

namespace N3O.Umbraco.Crowdfunding.Extensions; 

public static class CrowdfundingAllocationExtensions {
    public static CrowdfundingData GetCrowdfundingData(this IAllocation allocation, IJsonProvider jsonProvider) {
        if (!HasCrowdfundingData(allocation)) {
            return null;
        }

        return allocation.Extensions.Get<CrowdfundingData>(jsonProvider, Allocations.Extensions.Key);
    }
    
    public static bool HasCrowdfundingData(this IAllocation allocation) {
        return allocation.Extensions.ContainsKey(Allocations.Extensions.Key);
    }
    
    public static Allocation SetCrowdfundingData(this IAllocation allocation,
                                                 IJsonProvider jsonProvider,
                                                 CrowdfundingData extensionData) {
        allocation.Extensions.Set(jsonProvider, Allocations.Extensions.Key, extensionData);

        return new Allocation(allocation);
    }
}