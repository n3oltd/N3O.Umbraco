using N3O.Giving.Models;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Financial;
using N3O.Umbraco.Giving.Models;

namespace N3O.Umbraco.Giving {
    public class PricedAmountValidator : IPricedAmountValidator {
        private readonly IPriceCalculator _priceCalculator;

        public PricedAmountValidator(IPriceCalculator priceCalculator) {
            _priceCalculator = priceCalculator;
        }
        
        public bool IsValid(Money value, IPricing pricing, IFundDimensionValues fundDimensions) {
            var price = _priceCalculator.InCurrency(pricing, fundDimensions, value.Currency);

            if (price.HasValue() && price.Locked && price.Amount != value.Amount) {
                return false;
            }

            return true;
        }
    }
}