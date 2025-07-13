using N3O.Umbraco.Extensions;
using N3O.Umbraco.Giving.Allocations.Models;
using System.Collections.Generic;
using System.Linq;

namespace N3O.Umbraco.Giving.Allocations.Extensions;

public static class FundDimensionsOptionsExtensions {
    public static FundDimension1Value DefaultFundDimension1(this IFundDimensionOptions fundDimensionOptions) {
        return DefaultFundDimension(fundDimensionOptions.Dimension1);
    }

    public static FundDimension2Value DefaultFundDimension2(this IFundDimensionOptions fundDimensionOptions) {
        return DefaultFundDimension(fundDimensionOptions.Dimension2);
    }

    public static FundDimension3Value DefaultFundDimension3(this IFundDimensionOptions fundDimensionOptions) {
        return DefaultFundDimension(fundDimensionOptions.Dimension3);
    }

    public static FundDimension4Value DefaultFundDimension4(this IFundDimensionOptions fundDimensionOptions) {
        return DefaultFundDimension(fundDimensionOptions.Dimension4);
    }
    
    private static T DefaultFundDimension<T>(IEnumerable<T> fundDimensionValues) where T : FundDimensionValue<T> {
        var values = fundDimensionValues.OrEmpty().ToList();

        if (values.IsSingle()) {
            return values.Single();
        }
    
        return values.FirstOrDefault(x => x.IsUnrestricted);
    }
}
