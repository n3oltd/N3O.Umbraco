using N3O.Umbraco.Financial;
using N3O.Umbraco.Giving.Allocations.Models;

namespace N3O.Umbraco.Giving.Allocations;

public interface IPricedAmountValidator {
    bool IsValid(Money value, IPricing pricing, IFundDimensionValues fundDimensions, decimal multiplier = 1m);
}
