using N3O.Umbraco.Extensions;
using N3O.Umbraco.Financial;
using N3O.Umbraco.Forex;
using N3O.Umbraco.Giving.Content;
using N3O.Umbraco.Lookups;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace N3O.Umbraco.Crowdfunding.Models;

public class CrowdfunderPriceHandleViewModel {
    public Money Amount { get; private set; }
    public string Description { get; private set; }
    public IReadOnlyDictionary<string, Money> CurrencyAmountValues { get; private set; }

    public static async Task<CrowdfunderPriceHandleViewModel> ForAsync(Currency crowdfunderCurrency,
                                                                       IForexConverter forexConverter,
                                                                       ILookups lookups,
                                                                       PriceHandleElement fundraiserPriceHandle) {
        var viewModel = new CrowdfunderPriceHandleViewModel();

        viewModel.Amount = new Money(fundraiserPriceHandle.Amount, crowdfunderCurrency);
        viewModel.CurrencyAmountValues =  await GetCurrencyValues(forexConverter,
                                                                  lookups.GetAll<Currency>(),
                                                                  new Money(fundraiserPriceHandle.Amount, crowdfunderCurrency));
        viewModel.Description = fundraiserPriceHandle.Description;

        return viewModel;
    }
    
    private static async Task<Dictionary<string, Money>> GetCurrencyValues(IForexConverter forexConverter,
                                                                           IReadOnlyList<Currency> currencies,
                                                                           Money minimumValue) {
        var baseValue = await GetBaseValue(forexConverter, currencies, minimumValue);
        
        var currencyValues = new Dictionary<string, Money>();
        
        currencyValues.Add(baseValue.Currency.Code, new Money(baseValue.Amount, baseValue.Currency));

        foreach (var currency in currencies.Except(baseValue.Currency)) {
            var forexMoney = await forexConverter.BaseToQuote().ToCurrency(currency).ConvertAsync(baseValue.Amount);
            
            currencyValues.Add(currency.Code, new Money(forexMoney.Quote.Amount.RoundMoney(), currency));
        }

        return currencyValues;
    }
    
    private static async Task<Money> GetBaseValue(IForexConverter forexConverter,
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