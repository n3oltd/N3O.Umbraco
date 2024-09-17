using N3O.Umbraco.Financial;
using N3O.Umbraco.Giving.Content;

namespace N3O.Umbraco.Crowdfunding.Models;

public class CrowdfunderPriceHandleViewModel {
    public Money Amount { get; private set; }
    public string Description { get; private set; }

    public static CrowdfunderPriceHandleViewModel For(Currency crowdfunderCurrency,
                                                      PriceHandleElement fundraiserPriceHandle) {
        var viewModel = new CrowdfunderPriceHandleViewModel();

        viewModel.Amount = new Money(fundraiserPriceHandle.Amount, crowdfunderCurrency);
        viewModel.Description = fundraiserPriceHandle.Description;

        return viewModel;
    }
}