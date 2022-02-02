namespace N3O.Giving.Models {
    public interface IFundDimensionValues {
        FundDimension1Value Dimension1 { get; }
        FundDimension2Value Dimension2 { get; }
        FundDimension3Value Dimension3 { get; }
        FundDimension4Value Dimension4 { get; }
    }
}