using System.Collections.Generic;

namespace N3O.Umbraco.Giving.Allocations.Models;

public interface IFundDimensionOptions {
    IEnumerable<FundDimension1Value> Dimension1 { get; }
    IEnumerable<FundDimension2Value> Dimension2 { get; }
    IEnumerable<FundDimension3Value> Dimension3 { get; }
    IEnumerable<FundDimension4Value> Dimension4 { get; }
}
