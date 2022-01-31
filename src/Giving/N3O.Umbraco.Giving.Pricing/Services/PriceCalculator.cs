using N3O.Umbraco.Context;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Financial;
using N3O.Umbraco.Forex;
using N3O.Umbraco.Giving.Pricing.Models;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Giving.Pricing {
    public class PriceCalculator : IPriceCalculator {
        private readonly IBaseCurrencyAccessor _baseCurrencyAccessor;
        private readonly ICurrencyAccessor _currencyAccessor;
        private readonly IForexConverter _forexConverter;

        public PriceCalculator(IBaseCurrencyAccessor baseCurrencyAccessor,
                               ICurrencyAccessor currencyAccessor,
                               IForexConverter forexConverter) {
            _baseCurrencyAccessor = baseCurrencyAccessor;
            _currencyAccessor = currencyAccessor;
            _forexConverter = forexConverter;
        }
    
        public Money InBaseCurrency(IPrice price) {
            var baseCurrency = _baseCurrencyAccessor.GetBaseCurrency();

            return new Money(price.Amount, baseCurrency);
        }

        public Money InCurrentCurrency(IPrice price) {
            return InCurrentCurrencyAsync(price).GetAwaiter().GetResult();
        }

        public Task<Money> InCurrentCurrencyAsync(IPrice price, CancellationToken cancellationToken = default) {
            var currentCurrency = _currencyAccessor.GetCurrency();

            return InCurrencyAsync(price, currentCurrency, cancellationToken);
        }

        public Money InCurrency(IPrice price, Currency currency) {
            return InCurrencyAsync(price, currency).GetAwaiter().GetResult();
        }

        public async Task<Money> InCurrencyAsync(IPrice price,
                                                 Currency currency,
                                                 CancellationToken cancellationToken = default) {
            var forexMoney = await _forexConverter.BaseToQuote()
                                                  .ToCurrency(currency)
                                                  .ConvertAsync(price.Amount, cancellationToken);

            return forexMoney.Quote.RoundUpToWholeNumber();
        }
    }
}
