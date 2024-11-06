using N3O.Umbraco.Crowdfunding.Content;
using N3O.Umbraco.Crowdfunding.Entities;
using N3O.Umbraco.Lookups;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace N3O.Umbraco.Crowdfunding.Models;

public class SearchFundraisersViewModel : CrowdfundingViewModel {
    public IReadOnlyList<CrowdfunderTagViewModel> ActiveTags { get; private set; }
    public IReadOnlyList<CrowdfunderCardViewModel> Results { get; private set; }
    public string SearchTerm { get; private set; }
    
    public static async Task<SearchFundraisersViewModel> ForAsync(ICrowdfundingViewModelFactory viewModelFactory,
                                                                  ILookups lookups,
                                                                  ICrowdfundingUrlBuilder urlBuilder,
                                                                  SearchFundraisersPage page,
                                                                  IReadOnlyDictionary<string, string> query,
                                                                  IReadOnlyList<TagContent> activeTags,
                                                                  IReadOnlyList<Crowdfunder> results,
                                                                  string searchTerm) {
        var viewModel = await viewModelFactory.CreateViewModelAsync<SearchFundraisersViewModel>(page, query);
        viewModel.ActiveTags = activeTags.Select(x => CrowdfunderTagViewModel.For(urlBuilder, x)).ToList();
        viewModel.Results = results.Select(x => CrowdfunderCardViewModel.For(lookups, x)).ToList();
        viewModel.SearchTerm = searchTerm;

        return viewModel;
    }
}