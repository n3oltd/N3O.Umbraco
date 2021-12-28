using N3O.Umbraco.Financial;
using N3O.Umbraco.Giving.Pricing.Models;
using System.Threading.Tasks;

namespace N3O.Umbraco.Giving.Pricing;

public interface IPricing {
    Money InBaseCurrency(IHoldPrice item);
    Task<Money> InCurrentCurrencyAsync(IHoldPrice item);
    Task<Money> InCurrencyAsync(IHoldPrice item, Currency currency);
}
