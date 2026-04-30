using N3O.Umbraco.Extensions;
using N3O.Umbraco.Giving.Allocations.Models;
using System.Linq;

namespace N3O.Umbraco.Giving.Allocations.Extensions;

public static class FundDimensionsValuesExtensions {
    public static FundDimensionOptions ToFundDimensionOptions(this IFundDimensionValues fundDimensionValues) {
        if (fundDimensionValues == null) {
            return null;
        }
        
        return new FundDimensionOptions(new[] {fundDimensionValues.Dimension1}.ExceptNull().ToList(),
                                        new[] {fundDimensionValues.Dimension2}.ExceptNull().ToList(),
                                        new[] {fundDimensionValues.Dimension3}.ExceptNull().ToList(),
                                        new[] {fundDimensionValues.Dimension4}.ExceptNull().ToList());
    }
}
