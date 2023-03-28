namespace N3O.Umbraco.Giving.Models;

public class FundDimensionValuesRes : IFundDimensionValues {
    public FundDimension1Value Dimension1 { get; set; }
    public FundDimension2Value Dimension2 { get; set; }
    public FundDimension3Value Dimension3 { get; set; }
    public FundDimension4Value Dimension4 { get; set; }
}
