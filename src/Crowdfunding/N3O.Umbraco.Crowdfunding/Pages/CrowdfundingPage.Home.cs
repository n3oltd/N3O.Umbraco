using N3O.Umbraco.Content;
using N3O.Umbraco.Crowdfunding.Models;
using N3O.Umbraco.OpenGraph;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace N3O.Umbraco.Crowdfunding;

public class HomePage : CrowdfundingPage {
    public HomePage(IContentLocator contentLocator, ICrowdfundingViewModelFactory viewModelFactory)
        : base(contentLocator, viewModelFactory) { }

    protected override bool IsMatch(string crowdfundingPath, IReadOnlyDictionary<string, string> query) {
        return crowdfundingPath == CrowdfundingConstants.Routes.HomePage;
    }
    
    protected override void AddOpenGraph(IOpenGraphBuilder builder,
                                         string crowdfundingPath,
                                         IReadOnlyDictionary<string, string> query) {
        // TODO
    }

    protected override async Task<ICrowdfundingViewModel> GetViewModelAsync(string crowdfundingPath,
                                                                            IReadOnlyDictionary<string, string> query) {
        var viewModel = await HomeViewModel.ForAsync(ViewModelFactory, this, query);

        return viewModel;
    }
    
    public static string Url(IContentLocator contentLocator) {
        return GenerateUrl(contentLocator, null);
    }
}