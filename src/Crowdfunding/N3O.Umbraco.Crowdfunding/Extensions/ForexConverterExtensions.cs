using N3O.Umbraco.Extensions;
using N3O.Umbraco.Financial;
using N3O.Umbraco.Forex;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace N3O.Umbraco.Crowdfunding.Extensions;

public static class ForexConverterExtensions {
    public static async Task<Dictionary<Currency, Money>> GetCurrencyValuesAsync(this IForexConverter forexConverter,
                                                                                 IReadOnlyList<Currency> currencies,
                                                                                 Money value) {
        var baseValue = await GetBaseValueAsync(forexConverter, currencies, value);
        
        var currencyValues = new Dictionary<Currency, Money>();
        
        currencyValues.Add(baseValue.Currency, new Money(baseValue.Amount, baseValue.Currency));

        foreach (var currency in currencies.Except(baseValue.Currency)) {
            var forexMoney = await forexConverter.BaseToQuote().ToCurrency(currency).ConvertAsync(baseValue.Amount);
            
            currencyValues.Add(currency, new Money(forexMoney.Quote.Amount.RoundMoney(), currency));
        }

        return currencyValues;
    }
    
    private static async Task<Money> GetBaseValueAsync(IForexConverter forexConverter,
                                                       IReadOnlyList<Currency> currencies,
                                                       Money minimumValue) {
        Money baseValue;

        if (currencies.Single(x => x.IsBaseCurrency) == minimumValue.Currency) {
            baseValue = minimumValue;
        } else {
            var forexValue = await forexConverter.QuoteToBase()
                                                 .FromCurrency(minimumValue.Currency)
                                                 .ConvertAsync(minimumValue.Amount);
            
            baseValue = new Money(forexValue.Base.Amount.RoundMoney(), forexValue.Base.Currency);
        }

        return baseValue;
    }
}