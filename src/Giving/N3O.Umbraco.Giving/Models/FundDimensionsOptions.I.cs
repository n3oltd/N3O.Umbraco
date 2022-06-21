using System.Collections.Generic;

namespace N3O.Giving.Models;

public interface IFundDimensionsOptions {
    IEnumerable<FundDimension1Value> Dimension1Options { get; }
    IEnumerable<FundDimension2Value> Dimension2Options { get; }
    IEnumerable<FundDimension3Value> Dimension3Options { get; }
    IEnumerable<FundDimension4Value> Dimension4Options { get; }
}
