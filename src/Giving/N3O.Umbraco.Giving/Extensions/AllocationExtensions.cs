using N3O.Umbraco.Giving.Models;

namespace N3O.Umbraco.Giving.Extensions;

public static class AllocationExtensions {
    public static IFundDimensionsOptions GetFundDimensionsOptions(this IAllocation allocation) {
        return (IFundDimensionsOptions) allocation.Fund?.DonationItem ??
               (IFundDimensionsOptions) allocation.Sponsorship?.Scheme ??
               (IFundDimensionsOptions) allocation.Feedback?.Scheme;
    }
}