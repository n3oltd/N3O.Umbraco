using N3O.Umbraco.Content;
using N3O.Umbraco.Crowdfunding;
using N3O.Umbraco.Crowdfunding.Content;
using N3O.Umbraco.CrowdFunding.Models;
using System.Threading.Tasks;

namespace N3O.Umbraco.CrowdFunding;

public class CreatePage : CrowdfundingPage {
    private readonly IContentLocator _contentLocator;

    public CreatePage(ICrowdfundingHelper crowdfundingHelper, IContentLocator contentLocator)
        : base(crowdfundingHelper) {
        _contentLocator = contentLocator;
    }

    protected override bool IsMatch(string crowdfundingPath) {
        return IsMatch(crowdfundingPath, CrowdfundingUrl.Routes.Create);
    }

    protected override async Task<object> GetViewModelAsync(string crowdfundingPath) {
        var match = Match(crowdfundingPath, CrowdfundingUrl.Routes.Create);
        var campaignId = int.Parse(match.Groups[1].Value);

        var campaign = _contentLocator.ById<CampaignContent>(campaignId);

        var viewModel = CreatePageViewModel.For(campaign);

        return viewModel;
    }
}