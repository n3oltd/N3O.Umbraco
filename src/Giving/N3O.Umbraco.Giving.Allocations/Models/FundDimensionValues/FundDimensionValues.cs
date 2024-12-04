using Newtonsoft.Json;

namespace N3O.Umbraco.Giving.Allocations.Models;

public class FundDimensionValues : Value, IFundDimensionValues {
    [JsonConstructor]
    public FundDimensionValues(FundDimension1Value dimension1,
                               FundDimension2Value dimension2,
                               FundDimension3Value dimension3,
                               FundDimension4Value dimension4) {
        Dimension1 = dimension1;
        Dimension2 = dimension2;
        Dimension3 = dimension3;
        Dimension4 = dimension4;
    }

    public FundDimensionValues(IFundDimensionValues fundDimensionValues)
        : this(fundDimensionValues.Dimension1,
               fundDimensionValues.Dimension2,
               fundDimensionValues.Dimension3,
               fundDimensionValues.Dimension4) { }

    public FundDimension1Value Dimension1 { get; }
    public FundDimension2Value Dimension2 { get; }
    public FundDimension3Value Dimension3 { get; }
    public FundDimension4Value Dimension4 { get; }
}
