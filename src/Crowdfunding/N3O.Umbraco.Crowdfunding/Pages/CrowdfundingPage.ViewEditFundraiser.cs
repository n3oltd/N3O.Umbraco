using N3O.Umbraco.Content;
using N3O.Umbraco.Crowdfunding.Content;
using N3O.Umbraco.Crowdfunding.Models;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Localization;
using N3O.Umbraco.Lookups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace N3O.Umbraco.Crowdfunding;

public class ViewEditFundraiserPage : CrowdfundingPage {
    private readonly IContributionRepository _contributionRepository;
    private readonly ILookups _lookups;
    private readonly ITextFormatter _textFormatter;
    private readonly FundraiserAccessControl _fundraiserAccessControl;

    public ViewEditFundraiserPage(IContentLocator contentLocator,
                                  ICrowdfundingViewModelFactory viewModelFactory,
                                  IContributionRepository contributionRepository,
                                  ITextFormatter textFormatter,
                                  FundraiserAccessControl fundraiserAccessControl,
                                  ILookups lookups)
        : base(contentLocator, viewModelFactory) {
        _contributionRepository = contributionRepository;
        _textFormatter = textFormatter;
        _fundraiserAccessControl = fundraiserAccessControl;
        _lookups = lookups;
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
                                                          _lookups,
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