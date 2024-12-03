namespace N3O.Umbraco.Elements.Models;

public interface IPricingRule : IPrice {
    IFundDimensionValues FundDimensions { get; }
}
