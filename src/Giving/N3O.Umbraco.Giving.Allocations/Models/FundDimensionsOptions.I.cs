namespace N3O.Umbraco.Giving.Allocations.Models;

public interface IFundDimensionsOptions {
    IEnumerable<FundDimension1Value> Dimension1Options { get; }
    IEnumerable<FundDimension2Value> Dimension2Options { get; }
    IEnumerable<FundDimension3Value> Dimension3Options { get; }
    IEnumerable<FundDimension4Value> Dimension4Options { get; }
}
