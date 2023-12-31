using N3O.Umbraco.Giving.Models;
using System;

namespace N3O.Umbraco.Crowdfunding.Models;

// THe pattern for these is very similar to what we already have for block models and extension data in the N3O.Umbraco.Extensions
// project, e.g. see search for an example
public static class CrowdfundingAllocationExtensions {
    public static CrowdfundingData GetCrowdfundingData(this IAllocation allocation) {
        // Check allocation.Extensions for our key and retrieve it if exists
        throw new NotImplementedException();
    }
    
    public static Allocation SetCrowdfundingData(this IAllocation allocation, CrowdfundingData extensionData) {
        // Add or replace the extension data under the relevant key/
        throw new NotImplementedException();
    }

    public static bool HasCrowdfundingData(this IAllocation allocation) {
        throw new NotImplementedException();
    }
}

/*
 * E.g. in the cart view we can now do if (allocation.HasCrowdfundingData()) {
 *      fetch the data and use it to show the comment, page url or whatever else
 * }
*/