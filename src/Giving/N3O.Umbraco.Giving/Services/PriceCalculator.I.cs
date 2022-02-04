using N3O.Giving.Models;
using N3O.Umbraco.Financial;
using N3O.Umbraco.Giving.Models;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Giving {
    public interface IPriceCalculator {
        Price InBaseCurrency(IPricing pricing, IFundDimensionValues fundDimensions);

        Price InCurrency(IPricing pricing, IFundDimensionValues fundDimensions, Currency currency);
        
        Task<Price> InCurrencyAsync(IPricing pricing,
                                    IFundDimensionValues fundDimensions,
                                    Currency currency,
                                    CancellationToken cancellationToken = default);
    }
}
