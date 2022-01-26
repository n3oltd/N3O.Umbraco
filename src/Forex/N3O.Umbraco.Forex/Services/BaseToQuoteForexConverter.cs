using N3O.Umbraco.Context;
using N3O.Umbraco.Financial;
using NodaTime;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Forex {
    public class BaseToQuoteForexConverter {
        private readonly IExchangeRateCache _exchangeRateCache;
        private readonly IBaseCurrencyAccessor _baseCurrencyAccessor;
        private readonly ICurrencyAccessor _quoteCurrencyAccessor;
        private Currency _baseCurrency;
        private Currency _quoteCurrency;
        private LocalDate? _date;

        public BaseToQuoteForexConverter(IExchangeRateCache exchangeRateCache,
                                         IBaseCurrencyAccessor baseCurrencyAccessor,
                                         ICurrencyAccessor quoteCurrencyAccessor) {
            _exchangeRateCache = exchangeRateCache;
            _baseCurrencyAccessor = baseCurrencyAccessor;
            _quoteCurrencyAccessor = quoteCurrencyAccessor;
        }

        public BaseToQuoteForexConverter ToCurrency(Currency quoteCurrency) {
            _quoteCurrency = quoteCurrency;

            return this;
        }

        public BaseToQuoteForexConverter UsingRateOn(LocalDate date) {
            _date = date;

            return this;
        }

        public ForexMoney Convert(decimal baseAmount) {
            return ConvertAsync(baseAmount).GetAwaiter().GetResult();
        }

        public async Task<ForexMoney> ConvertAsync(decimal baseAmount, CancellationToken cancellationToken = default) {
            _quoteCurrency ??= _quoteCurrencyAccessor.GetCurrency();
            _baseCurrency ??= _baseCurrencyAccessor.GetBaseCurrency();

            decimal exchangeRate;

            if (_date == null) {
                exchangeRate = await _exchangeRateCache.GetLiveRateAsync(_baseCurrency,
                                                                         _quoteCurrency,
                                                                         cancellationToken);
            } else {
                exchangeRate = await _exchangeRateCache.GetHistoricalRateAsync(_date.Value,
                                                                               _baseCurrency,
                                                                               _quoteCurrency,
                                                                               cancellationToken);
            }

            var quoteAmount = baseAmount * exchangeRate;

            var baseMoney = new Money(baseAmount, _baseCurrency);
            var quoteMoney = new Money(quoteAmount, _quoteCurrency);

            var forex = new ForexMoney(baseMoney, quoteMoney, exchangeRate);

            return forex;
        }
    }
}
