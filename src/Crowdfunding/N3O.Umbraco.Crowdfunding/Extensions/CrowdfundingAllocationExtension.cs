using N3O.Umbraco.Crowdfunding.Models;
using N3O.Umbraco.Giving.Extensions;
using N3O.Umbraco.Giving.Models;
using N3O.Umbraco.Json;

namespace N3O.Umbraco.Crowdfunding.Extensions; 

public static class CrowdfundingAllocationExtensions {
    public static CrowdfundingData GetCrowdfundingData(this IAllocation allocation, IJsonProvider jsonProvider) {
        if (!HasCrowdfundingData(allocation)) {
            return null;
        }

        return allocation.Extensions.Get<CrowdfundingData>(jsonProvider, CrowdfundingConstants.CrowdfundingAllocation.Key);
    }
    
    public static Allocation SetCrowdfundingData(this IAllocation allocation, CrowdfundingData extensionData) {
        allocation.Extensions.Add(CrowdfundingConstants.CrowdfundingAllocation.Key, extensionData);

        return new Allocation(allocation);
    }

    public static bool HasCrowdfundingData(this IAllocation allocation) {
        return allocation.Extensions.ContainsKey(CrowdfundingConstants.CrowdfundingAllocation.Key);
    }
}