using N3O.Umbraco.Crowdfunding.Content;
using N3O.Umbraco.Crowdfunding.Extensions;
using N3O.Umbraco.Financial;
using N3O.Umbraco.Forex;
using N3O.Umbraco.Lookups;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace N3O.Umbraco.Crowdfunding.Models;

public class CreateFundraiserViewModel : CrowdfundingViewModel {
    public CampaignContent Campaign { get; private set; }
    public IReadOnlyList<Currency> Currencies { get; private set; }
    public IReadOnlyDictionary<Currency, Money> MinimumAmountValues { get; private set; }
    
    public static async Task<CreateFundraiserViewModel> ForAsync(ICrowdfundingViewModelFactory viewModelFactory,
                                                                 IForexConverter forexConverter,
                                                                 ILookups lookups,
                                                                 CreateFundraiserPage page,
                                                                 IReadOnlyDictionary<string, string> query,
                                                                 CampaignContent campaignContent) {
        var viewModel = await viewModelFactory.CreateViewModelAsync<CreateFundraiserViewModel>(page, query);
        viewModel.Campaign = campaignContent;
        viewModel.Currencies = lookups.GetAll<Currency>();
        viewModel.MinimumAmountValues = await forexConverter.GetCurrencyValuesAsync(viewModel.Currencies,
                                                                                     new Money(campaignContent.MinimumAmount, campaignContent.Currency));

        return viewModel;
    }
}