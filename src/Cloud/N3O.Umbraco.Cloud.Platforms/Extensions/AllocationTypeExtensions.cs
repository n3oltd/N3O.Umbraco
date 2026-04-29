using N3O.Umbraco.Exceptions;
using N3O.Umbraco.Giving.Allocations.Lookups;

namespace N3O.Umbraco.Cloud.Platforms.Extensions;

public static class AllocationTypeExtensions {
    public static string ToContentTypeAlias(this AllocationType allocationType) {
        if (allocationType == AllocationTypes.Feedback) {
            return PlatformsConstants.Offerings.Feedback;
        } else if (allocationType == AllocationTypes.Fund) {
            return PlatformsConstants.Offerings.Fund;
        } else if (allocationType == AllocationTypes.Qurbani) {
            return PlatformsConstants.Offerings.Qurbani;
        } else if (allocationType == AllocationTypes.Sponsorship) {
            return PlatformsConstants.Offerings.Sponsorship;
        }

        throw UnrecognisedValueException.For(allocationType);
    }
}
