using N3O.Giving.Models;

namespace N3O.Umbraco.Giving.Models;

public interface IPricingRule : IPrice {
    IFundDimensionValues FundDimensions { get; }
}
