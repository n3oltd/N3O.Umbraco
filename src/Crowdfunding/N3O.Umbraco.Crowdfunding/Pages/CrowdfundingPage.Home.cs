using N3O.Umbraco.Content;
using N3O.Umbraco.Crowdfunding.Content;
using N3O.Umbraco.Crowdfunding.Models;
using N3O.Umbraco.Lookups;
using N3O.Umbraco.OpenGraph;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace N3O.Umbraco.Crowdfunding;

public class HomePage : CrowdfundingPage {
    private readonly IContentLocator _contentLocator;
    private readonly ICrowdfunderRepository _crowdfunderRepository;
    private readonly ILookups _lookups;

    public HomePage(IContentLocator contentLocator,
                    ICrowdfunderRepository crowdfunderRepository,
                    ILookups lookups,
                    ICrowdfundingUrlBuilder urlBuilder,
                    ICrowdfundingViewModelFactory viewModelFactory)
        : base(contentLocator, urlBuilder, viewModelFactory) {
        _contentLocator = contentLocator;
        _crowdfunderRepository = crowdfunderRepository;
        _lookups = lookups;
    }

    protected override bool IsMatch(string crowdfundingPath, IReadOnlyDictionary<string, string> query) {
        return IsMatch(crowdfundingPath, CrowdfundingConstants.Routes.HomePage);
    }
    
    protected override void AddOpenGraph(IOpenGraphBuilder builder,
                                         string crowdfundingPath,
                                         IReadOnlyDictionary<string, string> query) {
        // TODO
    }

    protected override async Task<ICrowdfundingViewModel> GetViewModelAsync(string crowdfundingPath,
                                                                            IReadOnlyDictionary<string, string> query) {
        var featuredCampaigns = await _crowdfunderRepository.GetFeaturedCampaignsAsync();
        var almostCompleteFundraisers = await _crowdfunderRepository.GetAlmostCompleteFundraisersAsync(4);
        var newFundraisers = await _crowdfunderRepository.GetNewFundraisersAsync(4);

        var homepageTemplate = _contentLocator.Single<HomePageTemplateContent>();
        
        var viewModel = await HomeViewModel.ForAsync(ViewModelFactory,
                                                     _lookups,
                                                     this,
                                                     query,
                                                     featuredCampaigns,
                                                     almostCompleteFundraisers,
                                                     newFundraisers,
                                                     homepageTemplate);

        return viewModel;
    }
    
    public static string Url(ICrowdfundingUrlBuilder urlBuilder) {
        return urlBuilder.GenerateUrl(CrowdfundingConstants.Routes.HomePage);
    }
}