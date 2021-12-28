using N3O.Umbraco.Context;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Financial;
using N3O.Umbraco.Forex;
using N3O.Umbraco.Giving.Pricing.Models;
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
    
        public Task<Money> InCurrentCurrencyAsync(IHoldPrice item) {
            var currentCurrency = _currencyAccessor.GetCurrency();

            return InCurrencyAsync(item, currentCurrency);
        }

        public async Task<Money> InCurrencyAsync(IHoldPrice item, Currency currency) {
            var forexMoney = await _forexConverter.BaseToQuote()
                                                  .ToCurrency(currency)
                                                  .ConvertAsync(item.Price);

            return forexMoney.Quote.RoundUpToWholeNumber();
        }
    }
}
