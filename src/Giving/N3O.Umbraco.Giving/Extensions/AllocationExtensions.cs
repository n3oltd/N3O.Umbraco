using N3O.Umbraco.Extensions;
using N3O.Umbraco.Financial;
using N3O.Umbraco.Giving.Models;
using System.Collections.Generic;
using System.Linq;

namespace N3O.Umbraco.Giving.Extensions;

public static class AllocationExtensions {
    public static IFundDimensionsOptions GetFundDimensionsOptions(this IAllocation allocation) {
        return (IFundDimensionsOptions) allocation.Fund?.DonationItem ??
               (IFundDimensionsOptions) allocation.Sponsorship?.Scheme ??
               (IFundDimensionsOptions) allocation.Feedback?.Scheme;
    }
    
    public static Money GetTotalExcludingUpsells(this IEnumerable<Allocation> allocations, Currency currency) {
        allocations = allocations.OrEmpty().Where(x => !x.UpsellOfferId.HasValue()).ToList();
        
        if (allocations.Any()) {
            return allocations.Select(x => x.Value).Sum();
        } else {
            return currency.Zero();
        }
    }
}