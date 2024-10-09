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
    private readonly Lazy<IQueryStringAccessor> _queryStringAccessor;

    public ViewEditFundraiserPage(IContentLocator contentLocator,
                                  ICrowdfundingUrlBuilder urlBuilder,
                                  ICrowdfundingViewModelFactory viewModelFactory,
                                  IContributionRepository contributionRepository,
                                  ICurrencyAccessor currencyAccessor,
                                  ITextFormatter textFormatter,
                                  FundraiserAccessControl fundraiserAccessControl,
                                  IForexConverter forexConverter,
                                  ILookups lookups,
                                  Lazy<IQueryStringAccessor> queryStringAccessor)
        : base(contentLocator, urlBuilder, viewModelFactory) {
        _contributionRepository = contributionRepository;
        _currencyAccessor = currencyAccessor;
        _textFormatter = textFormatter;
        _fundraiserAccessControl = fundraiserAccessControl;
        _forexConverter = forexConverter;
        _lookups = lookups;
        _queryStringAccessor = queryStringAccessor;
    }

    protected override bool IsMatch(string crowdfundingPath, IReadOnlyDictionary<string, string> query) {
        if (!IsMatch(crowdfundingPath, CrowdfundingConstants.Routes.TypedRoutes.ViewEditFundraiser)) {
            return false;
        }
        
        var fundraiser = GetFundraiser(crowdfundingPath);

        if (!fundraiser.HasValue()) {
            return false;
        }
        
        var canEdit = _fundraiserAccessControl.CanEditAsync(fundraiser.Content()).GetAwaiter().GetResult();

        if (fundraiser.Status == CrowdfunderStatuses.Draft && !canEdit) {
            return false;
        }
        
        return true;
    }
    
    protected override void AddOpenGraph(IOpenGraphBuilder builder,
                                         string crowdfundingPath,
                                         IReadOnlyDictionary<string, string> query) {
        var fundraiser = GetFundraiser(crowdfundingPath);

        builder.WithTitle(fundraiser.Name);
        builder.WithDescription(fundraiser.Description);
        builder.WithUrl(Url(UrlBuilder, fundraiser.Key));
        builder.WithImagePath(fundraiser.OpenGraphImagePath);
    }

    protected override async Task<ICrowdfundingViewModel> GetViewModelAsync(string crowdfundingPath,
                                                                            IReadOnlyDictionary<string, string> query) {
        var fundraiser = GetFundraiser(crowdfundingPath);
        var contributions = await _contributionRepository.FindByFundraiserAsync(fundraiser.Content().Key);
        var viewMode = _queryStringAccessor.Value.GetValue(Parameters.ViewMode).HasValue();

        return await ViewEditFundraiserViewModel.ForAsync(ViewModelFactory,
                                                          _currencyAccessor,
                                                          _forexConverter,
                                                          _lookups,
                                                          _textFormatter,
                                                          _fundraiserAccessControl,
                                                          this,
                                                          query,
                                                          fundraiser,
                                                          contributions,
                                                          viewMode);
    }
    
    private FundraiserContent GetFundraiser(string crowdfundingPath) {
        var match = Match(crowdfundingPath, CrowdfundingConstants.Routes.TypedRoutes.ViewEditFundraiser);
        var fundraiserId = int.Parse(match.Groups[1].Value);
        var fundraiser = ContentLocator.ById<FundraiserContent>(fundraiserId);

        return fundraiser;
    }
    
    public static string Url(ICrowdfundingUrlBuilder urlBuilder, Guid fundraiserKey) {
        var fundraiser = urlBuilder.ContentLocator.ById<FundraiserContent>(fundraiserKey);
        
        if (fundraiser == null) {
            return null;
        }
        
        return urlBuilder.GenerateUrl(CrowdfundingConstants.Routes
                                                           .ViewEditFundraiser_2
                                                           .FormatWith(fundraiser.Content().Id, fundraiser.Slug));
    }
    
    private static class Parameters {
        public const string ViewMode = "view";
    }
}