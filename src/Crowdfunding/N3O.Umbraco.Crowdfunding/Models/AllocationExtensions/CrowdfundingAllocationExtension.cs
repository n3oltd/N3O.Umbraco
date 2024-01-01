using N3O.Umbraco.Extensions;
using N3O.Umbraco.Giving.Models;
using System;

namespace N3O.Umbraco.Crowdfunding.Models;

// THe pattern for these is very similar to what we already have for block models and extension data in the N3O.Umbraco.Extensions
// project, e.g. see search for an example
public static class CrowdfundingAllocationExtensions {
    public static CrowdfundingData GetCrowdfundingData(this IAllocation allocation) {
        // Check allocation.Extensions for our key and retrieve it if exists
        if (HasCrowdfundingData(allocation)) {
            return allocation.AllocationExtensionData.Get<CrowdfundingData>("crowdfunding");
        }

        return null;
    }
    
    public static Allocation SetCrowdfundingData(this IAllocation allocation, CrowdfundingData extensionData) {
        // Add or replace the extension data under the relevant key/
        allocation.AllocationExtensionData.JsonData["crowdfunding"] = new CrowdfundingData(extensionData);

        return new Allocation(allocation);
    }

    public static bool HasCrowdfundingData(this IAllocation allocation) {
        if (allocation.AllocationExtensionData.Get<CrowdfundingData>("crowdfunding").HasValue()) {
            return true;
        }

        return false;
    }
}