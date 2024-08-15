using N3O.Umbraco.Crowdfunding;
using N3O.Umbraco.CrowdFunding.Models;
using System.Threading.Tasks;

namespace N3O.Umbraco.CrowdFunding;

public class HomePage : CrowdfundingPage {
    public HomePage(ICrowdfundingHelper crowdfundingHelper) : base(crowdfundingHelper) { }

    protected override bool IsMatch(string crowdfundingPath) {
        return crowdfundingPath == string.Empty;
    }

    protected override Task<object> GetViewModelAsync(string crowdfundingPath) {
        var viewModel = HomeViewModel.For();

        return Task.FromResult<object>(viewModel);
    }
}