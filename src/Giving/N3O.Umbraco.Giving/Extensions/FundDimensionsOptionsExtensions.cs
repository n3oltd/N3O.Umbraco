using N3O.Umbraco.Extensions;
using N3O.Umbraco.Giving.Models;
using System.Collections.Generic;
using System.Linq;

namespace N3O.Umbraco.Giving.Extensions;

public static class FundDimensionsOptionsExtensions {
    public static FundDimension1Value DefaultFundDimension1(this IFundDimensionsOptions fundDimensionOptions) {
        return DefaultFundDimension(fundDimensionOptions.Dimension1Options);
    }

    public static FundDimension2Value DefaultFundDimension2(this IFundDimensionsOptions fundDimensionOptions) {
        return DefaultFundDimension(fundDimensionOptions.Dimension2Options);
    }

    public static FundDimension3Value DefaultFundDimension3(this IFundDimensionsOptions fundDimensionOptions) {
        return DefaultFundDimension(fundDimensionOptions.Dimension3Options);
    }

    public static FundDimension4Value DefaultFundDimension4(this IFundDimensionsOptions fundDimensionOptions) {
        return DefaultFundDimension(fundDimensionOptions.Dimension4Options);
    }
    
    private static T DefaultFundDimension<T>(IEnumerable<T> fundDimensionValues) where T : FundDimensionValue<T> {
        var values = fundDimensionValues.OrEmpty().ToList();

        if (values.IsSingle()) {
            return values.Single();
        }
    
        return values.FirstOrDefault(x => x.IsUnrestricted);
    }
}
