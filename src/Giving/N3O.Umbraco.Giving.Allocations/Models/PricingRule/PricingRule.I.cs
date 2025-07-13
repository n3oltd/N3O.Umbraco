namespace N3O.Umbraco.Giving.Allocations.Models;

public interface IPricingRule {
    IFundDimensionValues FundDimensions { get; }
    IPrice Price { get; }
}
