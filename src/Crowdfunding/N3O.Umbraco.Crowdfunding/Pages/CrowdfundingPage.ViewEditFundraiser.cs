using N3O.Umbraco.Content;
using N3O.Umbraco.Crowdfunding.Content;
using N3O.Umbraco.Crowdfunding.Models;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Localization;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace N3O.Umbraco.Crowdfunding;

public class ViewEditFundraiserPage : CrowdfundingPage {
    private readonly IContributionRepository _contributionRepository;
    private readonly ITextFormatter _textFormatter;
    private readonly FundraiserAccessControl _fundraiserAccessControl;

    public ViewEditFundraiserPage(IContentLocator contentLocator,
                                  ICrowdfundingViewModelFactory viewModelFactory,
                                  IContributionRepository contributionRepository,
                                  ITextFormatter textFormatter,
                                  FundraiserAccessControl fundraiserAccessControl)
        : base(contentLocator, viewModelFactory) {
        _contributionRepository = contributionRepository;
        _textFormatter = textFormatter;
        _fundraiserAccessControl = fundraiserAccessControl;
    }

    protected override bool IsMatch(string crowdfundingPath, IReadOnlyDictionary<string, string> query) {
        return IsMatch(crowdfundingPath, CrowdfundingConstants.Routes.Patterns.ViewEditFundraiser);
    }

    protected override async Task<ICrowdfundingViewModel> GetViewModelAsync(string crowdfundingPath,
                                                                            IReadOnlyDictionary<string, string> query) {
        var match = Match(crowdfundingPath, CrowdfundingConstants.Routes.Patterns.ViewEditFundraiser);
        var fundraiserId = int.Parse(match.Groups[1].Value);
        var fundraiser = ContentLocator.ById<FundraiserContent>(fundraiserId);

        var contributions = await _contributionRepository.FindByFundraiserAsync(fundraiser.Content().Key);

        return await ViewEditFundraiserViewModel.ForAsync(ViewModelFactory,
                                                          _textFormatter,
                                                          _fundraiserAccessControl,
                                                          this,
                                                          fundraiser,
                                                          contributions);
    }
    
    public static string Url(IContentLocator contentLocator, Guid fundraiserKey) {
        var fundraiser = contentLocator.ById<FundraiserContent>(fundraiserKey);
        
        return GenerateUrl(contentLocator, CrowdfundingConstants.Routes.ViewEditFundraiser_2.FormatWith(fundraiser.Content().Id,
                                                                                                        fundraiser.Slug));
    }
}