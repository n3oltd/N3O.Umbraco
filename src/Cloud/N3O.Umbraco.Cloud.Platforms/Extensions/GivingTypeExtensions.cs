using N3O.Umbraco.Cloud.Platforms.Lookups;
using N3O.Umbraco.Exceptions;
using N3O.Umbraco.Giving.Allocations.Lookups;

namespace N3O.Umbraco.Cloud.Platforms.Extensions;

public static class GivingTypeExtensions {
    public static GiftType ToGiftType(this GivingType givingType) {
        if (givingType == GivingTypes.Donation) {
            return GiftTypes.OneTime;
        } else if (givingType == GivingTypes.RegularGiving) {
            return GiftTypes.Recurring;
        } else {
            throw UnrecognisedValueException.For(givingType);
        }
    }
}