using N3O.Umbraco.Crowdfunding.Extensions;
using N3O.Umbraco.Financial;
using N3O.Umbraco.Forex;
using N3O.Umbraco.Giving.Content;
using N3O.Umbraco.Lookups;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace N3O.Umbraco.Crowdfunding.Models;

public class CrowdfunderPriceHandleViewModel {
    public Money Amount { get; private set; }
    public string Description { get; private set; }
    public IReadOnlyDictionary<Currency, Money> CurrencyAmountValues { get; private set; }

    public static async Task<CrowdfunderPriceHandleViewModel> ForAsync(Currency crowdfunderCurrency,
                                                                       IForexConverter forexConverter,
                                                                       ILookups lookups,
                                                                       PriceHandleElement fundraiserPriceHandle) {
        var viewModel = new CrowdfunderPriceHandleViewModel();

        viewModel.Amount = new Money(fundraiserPriceHandle.Amount, crowdfunderCurrency);
        viewModel.CurrencyAmountValues =  await forexConverter.GetCurrencyValuesAsync(lookups.GetAll<Currency>(),
                                                                                      new Money(fundraiserPriceHandle.Amount, crowdfunderCurrency));
        viewModel.Description = fundraiserPriceHandle.Description;

        return viewModel;
    }
}