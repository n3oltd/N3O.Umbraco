using Newtonsoft.Json;
using System.Collections.Generic;

namespace N3O.Umbraco.Giving.Allocations.Models;

public class FundDimensionOptions : Value, IFundDimensionOptions {
    [JsonConstructor]
    public FundDimensionOptions(IEnumerable<FundDimension1Value> dimension1,
                                IEnumerable<FundDimension2Value> dimension2,
                                IEnumerable<FundDimension3Value> dimension3,
                                IEnumerable<FundDimension4Value> dimension4) {
        Dimension1 = dimension1;
        Dimension2 = dimension2;
        Dimension3 = dimension3;
        Dimension4 = dimension4;
    }

    public FundDimensionOptions(IFundDimensionOptions fundDimensionOptions)
        : this(fundDimensionOptions.Dimension1,
               fundDimensionOptions.Dimension2,
               fundDimensionOptions.Dimension3,
               fundDimensionOptions.Dimension4) { }

    public IEnumerable<FundDimension1Value> Dimension1 { get; }
    public IEnumerable<FundDimension2Value> Dimension2 { get; }
    public IEnumerable<FundDimension3Value> Dimension3 { get; }
    public IEnumerable<FundDimension4Value> Dimension4 { get; }
}
