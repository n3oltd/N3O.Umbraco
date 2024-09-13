using N3O.Umbraco.Content;
using N3O.Umbraco.Crowdfunding;
using N3O.Umbraco.Crowdfunding.Content;
using N3O.Umbraco.CrowdFunding.Models;
using N3O.Umbraco.Localization;
using System.Threading.Tasks;

namespace N3O.Umbraco.CrowdFunding;

public class ViewEditFundraiserPage : CrowdfundingPage {
    private readonly IContentLocator _contentLocator;
    private readonly IOnlineContributionRepository _onlineContributionRepository;

    public ViewEditFundraiserPage(ICrowdfundingHelper crowdfundingHelper,
                                  IFormatter formatter,
                                  IContentLocator contentLocator,
                                  IOnlineContributionRepository onlineContributionRepository)
        : base(crowdfundingHelper, formatter) {
        _contentLocator = contentLocator;
        _onlineContributionRepository = onlineContributionRepository;
    }

    protected override bool IsMatch(string crowdfundingPath) {
        return IsMatch(crowdfundingPath, CrowdfundingUrl.Routes.ViewEditFundraiser);
    }

    protected override async Task<object> GetViewModelAsync(string crowdfundingPath) {
        var match = Match(crowdfundingPath, CrowdfundingUrl.Routes.ViewEditFundraiser);
        var fundraiserId = int.Parse(match.Groups[1].Value);
        var fundraiser = _contentLocator.ById<FundraiserContent>(fundraiserId);

        var contributions = await _onlineContributionRepository.FindByFundraiserAsync(fundraiser.Content().Key);

        return ViewEditFundraiserViewModel.For(CrowdfundingHelper, Formatter, fundraiser, contributions);
    }
}