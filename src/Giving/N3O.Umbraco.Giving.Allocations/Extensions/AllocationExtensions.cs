using N3O.Umbraco.Extensions;
using N3O.Umbraco.Financial;
using N3O.Umbraco.Giving.Allocations.Models;
using System.Collections.Generic;
using System.Linq;

namespace N3O.Umbraco.Giving.Allocations.Extensions; 

public static class AllocationExtensions {
    public static IFundDimensionOptions GetFundDimensionsOptions(this IAllocation allocation) {
        var holdFundDimensions = (IHoldFundDimensionOptions) allocation.Fund?.DonationItem ??
                                 (IHoldFundDimensionOptions) allocation.Sponsorship?.Scheme ??
                                 (IHoldFundDimensionOptions) allocation.Feedback?.Scheme;

        return holdFundDimensions.FundDimensionOptions;
    }
    
    public static Money GetTotalExcludingUpsells(this IEnumerable<Allocation> allocations, Currency currency) {
        allocations = allocations.OrEmpty().Where(x => !x.UpsellOfferId.HasValue()).ToList();
        
        if (allocations.Any()) {
            return allocations.Select(x => x.Value).Sum();
        } else {
            return currency.Zero();
        }
    }
    
    public static bool HasExtensionDataFor(this IAllocation allocation, string key) {
        return allocation.Extensions.HasFor(key);
    }
}