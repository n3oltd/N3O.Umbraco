using N3O.Umbraco.Attributes;

namespace N3O.Umbraco.Giving.Allocations.Models;

public class FundDimensionValuesReq : IFundDimensionValues {
    [Name("Dimension 1")]
    public FundDimension1Value Dimension1 { get; set; }

    [Name("Dimension 2")]
    public FundDimension2Value Dimension2 { get; set; }

    [Name("Dimension 3")]
    public FundDimension3Value Dimension3 { get; set; }

    [Name("Dimension 4")]
    public FundDimension4Value Dimension4 { get; set; }
}
