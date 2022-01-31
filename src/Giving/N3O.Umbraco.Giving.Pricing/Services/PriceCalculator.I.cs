using N3O.Umbraco.Financial;
using N3O.Umbraco.Giving.Pricing.Models;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Giving.Pricing {
    public interface IPriceCalculator {
        Money InBaseCurrency(IPrice price);
        Money InCurrentCurrency(IPrice price);
        Task<Money> InCurrentCurrencyAsync(IPrice price, CancellationToken cancellationToken = default);
        Money InCurrency(IPrice price, Currency currency);
        Task<Money> InCurrencyAsync(IPrice price, Currency currency, CancellationToken cancellationToken = default);
    }
}
