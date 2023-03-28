using N3O.Umbraco.Extensions;
using N3O.Umbraco.Financial;
using N3O.Umbraco.Giving.Models;

namespace N3O.Umbraco.Giving;

public class PricedAmountValidator : IPricedAmountValidator {
    private readonly IPriceCalculator _priceCalculator;

    public PricedAmountValidator(IPriceCalculator priceCalculator) {
        _priceCalculator = priceCalculator;
    }
    
    public bool IsValid(Money value, IPricing pricing, IFundDimensionValues fundDimensions, decimal multiplier = 1m) {
        var price = _priceCalculator.InCurrency(pricing, fundDimensions, value.Currency);

        var requiredAmount = price.Amount * multiplier;
        
        if (price.HasValue() && price.Locked && value.Amount < requiredAmount) {
            return false;
        }

        return true;
    }
}
