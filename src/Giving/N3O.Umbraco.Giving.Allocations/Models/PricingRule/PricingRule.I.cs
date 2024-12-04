namespace N3O.Umbraco.Giving.Allocations.Models;

public interface IPricingRule : IPrice {
    IFundDimensionValues FundDimensions { get; }
}
