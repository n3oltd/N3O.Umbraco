using N3O.Umbraco.Crowdfunding.Models;
using N3O.Umbraco.Giving.Extensions;
using N3O.Umbraco.Giving.Models;
using N3O.Umbraco.Json;
using static N3O.Umbraco.Crowdfunding.CrowdfundingConstants;

namespace N3O.Umbraco.Crowdfunding.Extensions; 

public static class AllocationExtensions {
    public static CrowdfunderData GetCrowdfunderData(this IAllocation allocation, IJsonProvider jsonProvider) {
        if (!HasCrowdfunderData(allocation)) {
            return null;
        }

        return allocation.Extensions.Get<CrowdfunderData>(jsonProvider, Allocations.Extensions.Key);
    }
    
    public static bool HasCrowdfunderData(this IAllocation allocation) {
        return allocation.Extensions.ContainsKey(Allocations.Extensions.Key);
    }
}