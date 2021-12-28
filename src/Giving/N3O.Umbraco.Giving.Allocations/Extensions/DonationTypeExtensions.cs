using N3O.Umbraco.Giving.Allocations.Lookups;
using System.Collections.Generic;
using System.Linq;

namespace N3O.Umbraco.Giving.Allocations.Extensions;

public static class DonationTypeExtensions {
    public static IEnumerable<DonationType> DefaultOrder(this IEnumerable<DonationType> source) {
        return source.OrderBy(x => x.IsSingle() ? 0 : 1);
    }

    public static bool IsSingle(this DonationType donationType) {
        return donationType == DonationTypes.Single;
    }

    public static bool IsRegular(this DonationType donationType) {
        return donationType == DonationTypes.Regular;
    }
}
