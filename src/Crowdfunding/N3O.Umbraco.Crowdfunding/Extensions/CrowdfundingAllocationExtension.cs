using N3O.Umbraco.Giving.Models;

namespace N3O.Umbraco.Crowdfunding.Models;

public static class CrowdfundingAllocationExtensions {
    public static CrowdfundingData GetCrowdfundingData(this IAllocation allocation) {
        if (!HasCrowdfundingData(allocation)) {
            return null;
        }

        return allocation.AllocationExtensionData.Get<CrowdfundingData>(CrowdfundingConstants.CrowdfundingAllocation.Key);
    }
    
    public static Allocation SetCrowdfundingData(this IAllocation allocation, CrowdfundingData extensionData) {
        allocation.AllocationExtensionData.Add(CrowdfundingConstants.CrowdfundingAllocation.Key, extensionData);

        return new Allocation(allocation);
    }

    public static bool HasCrowdfundingData(this IAllocation allocation) {
        return allocation.AllocationExtensionData.Fields.ContainsKey(CrowdfundingConstants.CrowdfundingAllocation.Key);
    }
}