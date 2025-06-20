using Flurl;
using N3O.Umbraco.Content;
using N3O.Umbraco.Crowdfunding.Content;
using N3O.Umbraco.Crowdfunding.Entities;
using N3O.Umbraco.Crowdfunding.Models;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Localization;
using N3O.Umbraco.Lookups;
using N3O.Umbraco.OpenGraph;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace N3O.Umbraco.Crowdfunding;

public class SearchFundraisersPage : CrowdfundingPage {
    private static readonly string TagParameterName = "tag";
    private static readonly string TextParameterName = "text";
    
    private readonly ICrowdfunderRepository _crowdfunderRepository;
    private readonly ILookups _lookups;

    public SearchFundraisersPage(IContentLocator contentLocator,
                                 IFormatter formatter,
                                 ICrowdfunderRepository crowdfunderRepository,
                                 ILookups lookups,
                                 ICrowdfundingUrlBuilder urlBuilder,
                                 ICrowdfundingViewModelFactory viewModelFactory)
        : base(contentLocator, formatter, urlBuilder, viewModelFactory) {
        _crowdfunderRepository = crowdfunderRepository;
        _lookups = lookups;
    }
    
    protected override string GetPageTitle(string crowdfundingPath, IReadOnlyDictionary<string, string> query) {
        return Formatter.Text.Format<Strings>(s => s.PageTitle);
    }
    
    protected override bool IsMatch(string crowdfundingPath, IReadOnlyDictionary<string, string> query) {
        return IsMatch(crowdfundingPath, CrowdfundingConstants.Routes.SearchFundraisers);
    }
    
    protected override void AddOpenGraph(IOpenGraphBuilder builder,
                                         string crowdfundingPath,
                                         IReadOnlyDictionary<string, string> query) {
        // TODO
    }

    protected override async Task<ICrowdfundingViewModel> GetViewModelAsync(string crowdfundingPath,
                                                                            IReadOnlyDictionary<string, string> query) {
        var activeTagNames = await _crowdfunderRepository.GetActiveFundraiserTagsAsync();
        var activeTags = ContentLocator.All<TagContent>().Where(x => activeTagNames.Contains(x.Name, true)).ToList();
        
        var tag = query.GetOrDefault("tag");
        var text = query.GetOrDefault("text");

        IReadOnlyList<Crowdfunder> results;

        if (tag.HasValue()) {
            results = await _crowdfunderRepository.FindFundraisersWithTagAsync(tag);
        } else if (text.HasValue()) {
            results = await _crowdfunderRepository.FindFundraisersAsync(text);
        } else {
            results = [];
        }
        
        var viewModel = await SearchFundraisersViewModel.ForAsync(ViewModelFactory,
                                                                  _lookups,
                                                                  UrlBuilder,
                                                                  this,
                                                                  query,
                                                                  activeTags,
                                                                  results,
                                                                  tag ?? text);

        return viewModel;
    }
    
    public static string Url(ICrowdfundingUrlBuilder urlBuilder, string tagName = null, string text = null) {
        var url = new Url(urlBuilder.GenerateUrl(CrowdfundingConstants.Routes.SearchFundraisers));

        if (tagName.HasValue()) {
            url.AppendQueryParam(TagParameterName, tagName);
        } else if (text.HasValue()) {
            url.AppendQueryParam(TextParameterName, text);
        }

        return url;
    }
    
    public class Strings : CodeStrings {
        public string PageTitle => "Search";
    }
}