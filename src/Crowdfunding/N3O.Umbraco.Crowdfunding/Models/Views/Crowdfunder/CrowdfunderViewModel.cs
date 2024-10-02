using N3O.Umbraco.Context;
using N3O.Umbraco.Crowdfunding.Content;
using N3O.Umbraco.Crowdfunding.Entities;
using N3O.Umbraco.Crowdfunding.Lookups;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Financial;
using N3O.Umbraco.Forex;
using N3O.Umbraco.Lookups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace N3O.Umbraco.Crowdfunding.Models;

public abstract class CrowdfunderViewModel<TContent> :
    CrowdfundingViewModel, ICrowdfunderViewModel
    where TContent : CrowdfunderContent<TContent> {
    public TContent Content { get; private set; }
    public CrowdfunderType CrowdfunderType { get; private set; }
    public Currency SiteCurrency { get; private set; }
    public IReadOnlyList<CrowdfunderGoalViewModel> Goals { get; private set; }
    public IReadOnlyList<CrowdfunderContributionViewModel> Contributions { get; private set; }
    public IReadOnlyList<CrowdfunderTagViewModel> Tags { get; private set; }
    public IReadOnlyList<Currency> Currencies { get; private set; }
    public CrowdfunderProgressViewModel CrowdfunderProgress { get; private set; }
    public CrowdfunderOwnerViewModel OwnerInfo { get; private set; }
    
    ICrowdfunderContent ICrowdfunderViewModel.Content => Content;
    
    public abstract bool EditMode();
    
    protected static async Task<T> ForAsync<T>(ICrowdfundingViewModelFactory viewModelFactory,
                                               ICurrencyAccessor currencyAccessor,
                                               IForexConverter forexConverter,
                                               ILookups lookups,
                                               ICrowdfundingPage page,
                                               IReadOnlyDictionary<string, string> query,
                                               TContent content,
                                               CrowdfunderType crowdfunderType,
                                               IEnumerable<Contribution> contributions,
                                               Func<CrowdfunderOwnerViewModel> getOwnerInfo)
        where T : CrowdfunderViewModel<TContent>, new() {
        var viewModel = await viewModelFactory.CreateViewModelAsync<T>(page, query);
        viewModel.Content = content;
        viewModel.CrowdfunderType = crowdfunderType;
        viewModel.Currencies = lookups.GetAll<Currency>();
        viewModel.SiteCurrency = currencyAccessor.GetCurrency();
        viewModel.Goals = await content.Goals.ToReadOnlyListAsync(async x => await CrowdfunderGoalViewModel.ForAsync(content.Currency,
                                                                                              forexConverter,
                                                                                              lookups, 
                                                                                              x));
        viewModel.Contributions = contributions.ToReadOnlyList(x => CrowdfunderContributionViewModel.For(viewModel.Formatter,
                                                                                                         lookups,
                                                                                                         x));
        
        viewModel.Tags = content.Goals.SelectMany(x => x.Tags.Select(CrowdfunderTagViewModel.For)).ToList();
        viewModel.CrowdfunderProgress = CrowdfunderProgressViewModel.For(content.Currency,
                                                                         contributions,
                                                                         content.Goals);
        viewModel.OwnerInfo = getOwnerInfo();
        
        return viewModel;
    }
}