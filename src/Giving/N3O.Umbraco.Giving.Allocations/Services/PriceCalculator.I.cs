using N3O.Umbraco.Financial;
using N3O.Umbraco.Giving.Allocations.Models;

namespace N3O.Umbraco.Giving.Allocations;

public interface IPriceCalculator {
    Price InBaseCurrency(IPricing pricing, IFundDimensionValues fundDimensions);

    Price InCurrency(IPricing pricing, IFundDimensionValues fundDimensions, Currency currency);
    
    Task<Price> InCurrencyAsync(IPricing pricing,
                                IFundDimensionValues fundDimensions,
                                Currency currency,
                                CancellationToken cancellationToken = default);
}
