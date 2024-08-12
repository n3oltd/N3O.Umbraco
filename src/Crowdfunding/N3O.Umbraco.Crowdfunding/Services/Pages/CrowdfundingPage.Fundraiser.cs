using N3O.Umbraco.Content;
using N3O.Umbraco.Crowdfunding;
using N3O.Umbraco.Crowdfunding.Content;
using N3O.Umbraco.CrowdFunding.Models.FundraisingPage;
using System.Threading.Tasks;

namespace N3O.Umbraco.CrowdFunding;

public class FundraiserPage : CrowdfundingPage {
    private readonly IContentLocator _contentLocator;
    private readonly IContributionRepository _contributionRepository;

    public FundraiserPage(ICrowdfundingHelper crowdfundingHelper,
                          IContentLocator contentLocator,
                          IContributionRepository contributionRepository)
        : base(crowdfundingHelper) {
        _contentLocator = contentLocator;
        _contributionRepository = contributionRepository;
    }

    protected override bool IsMatch(string crowdfundingPath) {
        return IsMatch(crowdfundingPath, CrowdfundingConstants.Routes.Fundraiser);
    }

    protected override async Task<object> GetViewModelAsync(string crowdfundingPath) {
        var match = Match(crowdfundingPath, CrowdfundingConstants.Routes.Fundraiser);
        var pageId = int.Parse(match.Groups[1].Value);
        var fundraiser = _contentLocator.ById<FundraiserContent>(pageId);

        var contributions = await _contributionRepository.FindByFundraiserAsync(fundraiser.Content().Key);

        return FundraiserViewModel.For(CrowdfundingHelper, fundraiser, contributions);
    }
}