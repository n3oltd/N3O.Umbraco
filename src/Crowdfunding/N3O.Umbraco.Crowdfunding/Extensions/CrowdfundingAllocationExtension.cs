using N3O.Umbraco.Giving.Models;
using N3O.Umbraco.Json;

namespace N3O.Umbraco.Crowdfunding.Models;

public static class CrowdfundingAllocationExtensions {
    public static CrowdfundingData GetCrowdfundingData(this IAllocation allocation, IJsonProvider jsonProvider) {
        if (!HasCrowdfundingData(allocation)) {
            return null;
        }

        return allocation.AllocationExtensionData.Get<CrowdfundingData>(jsonProvider, CrowdfundingConstants.CrowdfundingAllocation.Key);
    }
    
    public static Allocation SetCrowdfundingData(this IAllocation allocation, CrowdfundingData extensionData) {
        allocation.AllocationExtensionData.Add(CrowdfundingConstants.CrowdfundingAllocation.Key, extensionData);

        return new Allocation(allocation);
    }

    public static bool HasCrowdfundingData(this IAllocation allocation) {
        return allocation.AllocationExtensionData.ContainsKey(CrowdfundingConstants.CrowdfundingAllocation.Key);
    }
}