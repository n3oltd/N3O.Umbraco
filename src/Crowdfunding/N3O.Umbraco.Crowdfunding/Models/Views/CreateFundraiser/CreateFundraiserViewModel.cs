using N3O.Umbraco.Crowdfunding.Content;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Financial;
using N3O.Umbraco.Forex;
using N3O.Umbraco.Lookups;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace N3O.Umbraco.Crowdfunding.Models;

public class CreateFundraiserViewModel : CrowdfundingViewModel {
    public CampaignContent Campaign { get; private set; }
    public IReadOnlyList<Currency> Currencies { get; private set; }
    public IReadOnlyDictionary<string, Money> MinimumAmountValues { get; private set; }
    
    public static async Task<CreateFundraiserViewModel> ForAsync(ICrowdfundingViewModelFactory viewModelFactory,
                                                                 IForexConverter forexConverter,
                                                                 ILookups lookups,
                                                                 CreateFundraiserPage page,
                                                                 CampaignContent campaignContent) {
        var viewModel = await viewModelFactory.CreateViewModelAsync<CreateFundraiserViewModel>(page);
        viewModel.Campaign = campaignContent;
        viewModel.Currencies = lookups.GetAll<Currency>();
        viewModel.MinimumAmountValues =  await GetCurrencyValues(forexConverter,
                                                                 viewModel.Currencies,
                                                                 new Money(campaignContent.MinimumAmount, campaignContent.Currency));

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
            
            currencyValues.Add(currency.Code, forexMoney.Quote);
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
            
            baseValue = forexValue.Base;
        }

        return baseValue;
    }
}