using N3O.Umbraco.Content;
using N3O.Umbraco.Crowdfunding;
using N3O.Umbraco.Crowdfunding.Content;
using N3O.Umbraco.CrowdFunding.Models;
using System.Threading.Tasks;

namespace N3O.Umbraco.CrowdFunding;

public class CampaignPage : CrowdfundingPage {
    private readonly IContentLocator _contentLocator;
    private readonly IContributionRepository _contributionRepository;

    public CampaignPage(ICrowdfundingHelper crowdfundingHelper,
                          IContentLocator contentLocator,
                          IContributionRepository contributionRepository)
        : base(crowdfundingHelper) {
        _contentLocator = contentLocator;
        _contributionRepository = contributionRepository;
    }

    protected override bool IsMatch(string crowdfundingPath) {
        return IsMatch(crowdfundingPath, CrowdfundingUrl.Routes.Campaign);
    }

    protected override async Task<object> GetViewModelAsync(string crowdfundingPath) {
        var match = Match(crowdfundingPath, CrowdfundingUrl.Routes.Campaign);
        var campaignId = int.Parse(match.Groups[1].Value);
        var campaign = _contentLocator.ById<CampaignContent>(campaignId);

        var contributions = await _contributionRepository.FindByCampaignAsync(campaign.Content().Key);

        return CampaignViewModel.For(CrowdfundingHelper, campaign, contributions);
    }
}