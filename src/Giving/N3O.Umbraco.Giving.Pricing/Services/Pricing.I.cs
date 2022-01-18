using N3O.Umbraco.Financial;
using N3O.Umbraco.Giving.Pricing.Models;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Giving.Pricing {
    public interface IPricing {
        Money InBaseCurrency(IHoldPrice item);
        Money InCurrentCurrency(IHoldPrice item);
        Task<Money> InCurrentCurrencyAsync(IHoldPrice item, CancellationToken cancellationToken = default);
        Money InCurrency(IHoldPrice item, Currency currency);
        Task<Money> InCurrencyAsync(IHoldPrice item, Currency currency, CancellationToken cancellationToken = default);
    }
}
