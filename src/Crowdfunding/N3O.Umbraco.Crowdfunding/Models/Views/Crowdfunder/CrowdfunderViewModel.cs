using N3O.Umbraco.Crm.Lookups;
using N3O.Umbraco.Crowdfunding.Content;
using N3O.Umbraco.Crowdfunding.Entities;
using N3O.Umbraco.Extensions;
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
    public IReadOnlyList<CrowdfunderGoalViewModel> Goals { get; private set; }
    public IReadOnlyList<CrowdfunderContributionViewModel> Contributions { get; private set; }
    public IReadOnlyList<string> Tags { get; private set; }
    public CrowdfunderProgressViewModel CrowdfunderProgress { get; private set; }
    public CrowdfunderOwnerViewModel OwnerInfo { get; private set; }
    
    ICrowdfunderContent ICrowdfunderViewModel.Content => Content;
    
    public abstract bool EditMode();
    
    protected static async Task<T> ForAsync<T>(ICrowdfundingViewModelFactory viewModelFactory,
                                               ICrowdfundingPage page,
                                               TContent content,
                                               CrowdfunderType crowdfunderType,
                                               IEnumerable<Contribution> contributions,
                                               Func<CrowdfunderOwnerViewModel> getOwnerInfo)
        where T : CrowdfunderViewModel<TContent>, new() {
        var viewModel = await viewModelFactory.CreateViewModelAsync<T>(page);
        viewModel.Content = content;
        viewModel.CrowdfunderType = crowdfunderType;
        viewModel.Goals = content.Goals.ToReadOnlyList(x => CrowdfunderGoalViewModel.For(content.Currency, x));
        viewModel.Contributions = contributions.ToReadOnlyList(x => CrowdfunderContributionViewModel.For(viewModel.Formatter,
                                                                                                         content.Currency,
                                                                                                         x));
        viewModel.Tags = content.Goals.SelectMany(x => x.Tags.Select(t => t.Name)).ToList();
        viewModel.CrowdfunderProgress = CrowdfunderProgressViewModel.For(content.Currency,
                                                                         contributions,
                                                                         content.Goals);
        viewModel.OwnerInfo = getOwnerInfo();
        
        return viewModel;
    }
}