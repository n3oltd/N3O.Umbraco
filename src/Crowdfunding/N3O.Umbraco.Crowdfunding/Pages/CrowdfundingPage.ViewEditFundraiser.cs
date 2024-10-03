using N3O.Umbraco.Content;
using N3O.Umbraco.Context;
using N3O.Umbraco.Crm.Lookups;
using N3O.Umbraco.Crowdfunding.Content;
using N3O.Umbraco.Crowdfunding.Models;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Forex;
using N3O.Umbraco.Localization;
using N3O.Umbraco.Lookups;
using N3O.Umbraco.OpenGraph;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace N3O.Umbraco.Crowdfunding;

public class ViewEditFundraiserPage : CrowdfundingPage {
    private readonly IContributionRepository _contributionRepository;
    private readonly ICurrencyAccessor _currencyAccessor;
    private readonly IForexConverter _forexConverter;
    private readonly ILookups _lookups;
    private readonly ITextFormatter _textFormatter;
    private readonly FundraiserAccessControl _fundraiserAccessControl;

    public ViewEditFundraiserPage(IContentLocator contentLocator,
                                  ICrowdfundingViewModelFactory viewModelFactory,
                                  IContributionRepository contributionRepository,
                                  ICurrencyAccessor currencyAccessor,
                                  ITextFormatter textFormatter,
                                  FundraiserAccessControl fundraiserAccessControl,
                                  IForexConverter forexConverter,
                                  ILookups lookups)
        : base(contentLocator, viewModelFactory) {
        _contributionRepository = contributionRepository;
        _currencyAccessor = currencyAccessor;
        _textFormatter = textFormatter;
        _fundraiserAccessControl = fundraiserAccessControl;
        _forexConverter = forexConverter;
        _lookups = lookups;
    }

    protected override bool IsMatch(string crowdfundingPath, IReadOnlyDictionary<string, string> query) {
        var isMatch = IsMatch(crowdfundingPath, CrowdfundingConstants.Routes.Patterns.ViewEditFundraiser);

        if (!isMatch) {
            return false;
        }
        
        var fundraiser = GetFundraiser(crowdfundingPath);

        isMatch = fundraiser.HasValue() && fundraiser.Status != CrowdfunderStatuses.Draft;
        
        return isMatch;
    }
    
    protected override void AddOpenGraph(IOpenGraphBuilder builder,
                                         string crowdfundingPath,
                                         IReadOnlyDictionary<string, string> query) {
        var fundraiser = GetFundraiser(crowdfundingPath);

        builder.WithTitle(fundraiser.Name);
        builder.WithDescription(fundraiser.Description);
        builder.WithImageUrl(fundraiser.OpenGraphImageUrl);
    }

    protected override async Task<ICrowdfundingViewModel> GetViewModelAsync(string crowdfundingPath,
                                                                            IReadOnlyDictionary<string, string> query) {
        var fundraiser = GetFundraiser(crowdfundingPath);
        var contributions = await _contributionRepository.FindByFundraiserAsync(fundraiser.Content().Key);

        return await ViewEditFundraiserViewModel.ForAsync(ViewModelFactory,
                                                          _currencyAccessor,
                                                          _forexConverter,
                                                          _lookups,
                                                          _textFormatter,
                                                          _fundraiserAccessControl,
                                                          this,
                                                          query,
                                                          fundraiser,
                                                          contributions);
    }

    public static string Url(IContentLocator contentLocator, Guid fundraiserKey) {
        var fundraiser = contentLocator.ById<FundraiserContent>(fundraiserKey);
        
        return GenerateUrl(contentLocator, CrowdfundingConstants.Routes.ViewEditFundraiser_2.FormatWith(fundraiser.Content().Id,
                                                                                                        fundraiser.Slug));
    }
    
    private FundraiserContent GetFundraiser(string crowdfundingPath) {
        var match = Match(crowdfundingPath, CrowdfundingConstants.Routes.Patterns.ViewEditFundraiser);
        var fundraiserId = int.Parse(match.Groups[1].Value);
        var fundraiser = ContentLocator.ById<FundraiserContent>(fundraiserId);

        return fundraiser;
    }
}