using N3O.Umbraco.Giving.Allocations.Lookups;
using System.Collections.Generic;
using System.Linq;

namespace N3O.Umbraco.Giving.Allocations.Extensions;

public static class GivingTypeExtensions {
    public static IEnumerable<GivingType> DefaultOrder(this IEnumerable<GivingType> source) {
        return source.OrderBy(x => x.Order);
    }

    public static bool IsDonation(this GivingType givingType) {
        return givingType == GivingTypes.Donation;
    }

    public static bool IsRegularGiving(this GivingType givingType) {
        return givingType == GivingTypes.RegularGiving;
    }
}
