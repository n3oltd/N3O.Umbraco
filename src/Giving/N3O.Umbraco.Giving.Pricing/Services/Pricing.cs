using N3O.Umbraco.Context;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Financial;
using N3O.Umbraco.Forex;
using N3O.Umbraco.Giving.Pricing.Models;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Giving.Pricing {
    public class Pricing : IPricing {
        private readonly IBaseCurrencyAccessor _baseCurrencyAccessor;
        private readonly ICurrencyAccessor _currencyAccessor;
        private readonly IForexConverter _forexConverter;

        public Pricing(IBaseCurrencyAccessor baseCurrencyAccessor,
                       ICurrencyAccessor currencyAccessor,
                       IForexConverter forexConverter) {
            _baseCurrencyAccessor = baseCurrencyAccessor;
            _currencyAccessor = currencyAccessor;
            _forexConverter = forexConverter;
        }
    
        public Money InBaseCurrency(IHoldPrice item) {
            var baseCurrency = _baseCurrencyAccessor.GetBaseCurrency();

            return new Money(item.Price, baseCurrency);
        }

        public Money InCurrentCurrency(IHoldPrice item) {
            return InCurrentCurrencyAsync(item).GetAwaiter().GetResult();
        }

        public Task<Money> InCurrentCurrencyAsync(IHoldPrice item, CancellationToken cancellationToken = default) {
            var currentCurrency = _currencyAccessor.GetCurrency();

            return InCurrencyAsync(item, currentCurrency, cancellationToken);
        }

        public Money InCurrency(IHoldPrice item, Currency currency) {
            return InCurrencyAsync(item, currency).GetAwaiter().GetResult();
        }

        public async Task<Money> InCurrencyAsync(IHoldPrice item,
                                                 Currency currency,
                                                 CancellationToken cancellationToken = default) {
            var forexMoney = await _forexConverter.BaseToQuote()
                                                  .ToCurrency(currency)
                                                  .ConvertAsync(item.Price, cancellationToken);

            return forexMoney.Quote.RoundUpToWholeNumber();
        }
    }
}
