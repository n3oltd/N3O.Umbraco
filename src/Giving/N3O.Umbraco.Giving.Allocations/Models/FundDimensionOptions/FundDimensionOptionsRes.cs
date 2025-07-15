using System.Collections.Generic;

namespace N3O.Umbraco.Giving.Allocations.Models;

public class FundDimensionOptionsRes : IFundDimensionOptions {
    public IEnumerable<FundDimension1Value> Dimension1 { get; set; }
    public IEnumerable<FundDimension2Value> Dimension2 { get; set; }
    public IEnumerable<FundDimension3Value> Dimension3 { get; set; }
    public IEnumerable<FundDimension4Value> Dimension4 { get; set; }
}
