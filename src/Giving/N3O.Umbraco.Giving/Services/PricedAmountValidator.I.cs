using N3O.Giving.Models;
using N3O.Umbraco.Financial;
using N3O.Umbraco.Giving.Models;

namespace N3O.Umbraco.Giving {
    public interface IPricedAmountValidator {
        bool IsValid(Money value, IPricing pricing, IFundDimensionValues fundDimensions, decimal multiplier = 1m);
    }
}