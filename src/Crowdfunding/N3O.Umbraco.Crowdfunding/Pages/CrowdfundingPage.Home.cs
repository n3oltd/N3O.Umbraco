using N3O.Umbraco.Content;
using N3O.Umbraco.Crowdfunding.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace N3O.Umbraco.Crowdfunding;

public class CrowdfunderHomePage : CrowdfundingPage {
    public CrowdfunderHomePage(IContentLocator contentLocator, ICrowdfundingViewModelFactory viewModelFactory)
        : base(contentLocator, viewModelFactory) { }

    protected override bool IsMatch(string crowdfundingPath, IReadOnlyDictionary<string, string> query) {
        return crowdfundingPath == string.Empty;
    }

    protected override async Task<ICrowdfundingViewModel> GetViewModelAsync(string crowdfundingPath,
                                                                            IReadOnlyDictionary<string, string> query) {
        var viewModel = await HomeViewModel.ForAsync(ViewModelFactory, this);

        return viewModel;
    }
    
    public static string Url(IContentLocator contentLocator) {
        return GenerateUrl(contentLocator, null);
    }
}