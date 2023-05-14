using N3O.Umbraco.Extensions;
using N3O.Umbraco.Giving.Models;
using System;

namespace N3O.Umbraco.Giving.Extensions;

public static class AllocationExtensions {
    public static IFundDimensionsOptions GetFundDimensionsOptions(this IAllocation allocation) {
        if (allocation.Fund?.DonationItem != null) {
            return (IFundDimensionsOptions) allocation.Fund?.DonationItem;
        } else if (allocation.Sponsorship?.Scheme != null) {
            return (IFundDimensionsOptions) allocation.Sponsorship.Scheme;
        } else if (allocation.Feedback?.Scheme != null) {
            return (IFundDimensionsOptions) allocation.Feedback.Scheme;
        }

        throw new Exception("Invalid fund dimension options");
    }
}
