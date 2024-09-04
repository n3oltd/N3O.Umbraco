using N3O.Umbraco.Content;
using N3O.Umbraco.Crowdfunding;
using N3O.Umbraco.Crowdfunding.Content;
using N3O.Umbraco.CrowdFunding.Models;
using System.Threading.Tasks;

namespace N3O.Umbraco.CrowdFunding;

public class ViewCampaignPage : CrowdfundingPage {
    private readonly IContentLocator _contentLocator;
    private readonly IOnlineContributionRepository _onlineContributionRepository;

    public ViewCampaignPage(ICrowdfundingHelper crowdfundingHelper,
                            IContentLocator contentLocator,
                            IOnlineContributionRepository onlineContributionRepository)
        : base(crowdfundingHelper) {
        _contentLocator = contentLocator;
        _onlineContributionRepository = onlineContributionRepository;
    }

    protected override bool IsMatch(string crowdfundingPath) {
        return IsMatch(crowdfundingPath, CrowdfundingUrl.Routes.ViewCampaign);
    }

    protected override async Task<object> GetViewModelAsync(string crowdfundingPath) {
        var match = Match(crowdfundingPath, CrowdfundingUrl.Routes.ViewCampaign);
        var campaignId = int.Parse(match.Groups[1].Value);
        var campaign = _contentLocator.ById<CampaignContent>(campaignId);

        var contributions = await _onlineContributionRepository.FindByCampaignAsync(campaign.Content().Key);

        return ViewCampaignViewModel.For(CrowdfundingHelper, campaign, contributions);
    }
}