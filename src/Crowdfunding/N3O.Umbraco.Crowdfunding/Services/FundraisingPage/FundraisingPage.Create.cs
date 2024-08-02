using N3O.Umbraco.Content;
using N3O.Umbraco.Crowdfunding.Content;
using N3O.Umbraco.CrowdFunding.Models.FundraisingPage;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace N3O.Umbraco.CrowdFunding.Services;

public class FundraisingCreatePage : FundraisingPageBase {
    private readonly IContentLocator _contentLocator;

    public FundraisingCreatePage(IContentLocator contentLocator) {
        _contentLocator = contentLocator;
    }
    
    public override bool IsMatch(string path) {
        return Regex.IsMatch(path, FundraisingPageUrl.FundraisingCreatePagePath, RegexOptions.IgnoreCase);
    }

    public override Task<object> GetViewModelAsync(string path) {
        var crowdfundingCampaigns = _contentLocator.All<CrowdfundingCampaignContent>();

        var viewModel = new CrowdfundingCreatePageViewModel();
        viewModel.CrowdfundingCampaigns = crowdfundingCampaigns;

        return Task.FromResult<object>(viewModel);
    }
}