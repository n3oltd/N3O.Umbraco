using N3O.Umbraco.Cloud.Engage.Lookups;
using N3O.Umbraco.Content;
using N3O.Umbraco.Context;
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
using Umbraco.Cms.Core.Security;

namespace N3O.Umbraco.Crowdfunding;

public class ViewEditFundraiserPage : CrowdfundingPage {
    private readonly IContributionRepository _contributionRepository;
    private readonly ICurrencyAccessor _currencyAccessor;
    private readonly IForexConverter _forexConverter;
    private readonly ILookups _lookups;
    private readonly ITextFormatter _textFormatter;
    private readonly FundraiserAccessControl _fundraiserAccessControl;
    private readonly IQueryStringAccessor _queryStringAccessor;
    private readonly IMemberManager _memberManager;

    public ViewEditFundraiserPage(IContentLocator contentLocator,
                                  IFormatter formatter,
                                  ICrowdfundingUrlBuilder urlBuilder,
                                  ICrowdfundingViewModelFactory viewModelFactory,
                                  IContributionRepository contributionRepository,
                                  ICurrencyAccessor currencyAccessor,
                                  ITextFormatter textFormatter,
                                  FundraiserAccessControl fundraiserAccessControl,
                                  IForexConverter forexConverter,
                                  ILookups lookups,
                                  IQueryStringAccessor queryStringAccessor,
                                  IMemberManager memberManager)
        : base(contentLocator, formatter, urlBuilder, viewModelFactory) {
        _contributionRepository = contributionRepository;
        _currencyAccessor = currencyAccessor;
        _textFormatter = textFormatter;
        _fundraiserAccessControl = fundraiserAccessControl;
        _forexConverter = forexConverter;
        _lookups = lookups;
        _queryStringAccessor = queryStringAccessor;
        _memberManager = memberManager;
    }

    protected override string GetPageTitle(string crowdfundingPath, IReadOnlyDictionary<string, string> query) {
        var fundraiser = GetFundraiser(crowdfundingPath);

        return fundraiser.Name;
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

        if (fundraiser.Status == CrowdfunderStatuses.Draft && _memberManager.IsLoggedIn() && !canEdit) {
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
        var preview = _queryStringAccessor.Has(Parameters.Preview);

        return await ViewEditFundraiserViewModel.ForAsync(ViewModelFactory,
                                                          _currencyAccessor,
                                                          _forexConverter,
                                                          UrlBuilder,
                                                          _lookups,
                                                          _textFormatter,
                                                          _fundraiserAccessControl,
                                                          this,
                                                          query,
                                                          fundraiser,
                                                          contributions,
                                                          preview);
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
        public static readonly string Preview = "preview";
    }
}