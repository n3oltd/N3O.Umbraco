using Google.Apis.CustomSearchAPI.v1;
using Google.Apis.Services;
using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Search.Google.Content;
using N3O.Umbraco.Search.Google.Models;
using N3O.Umbraco.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Cms.Core.Cache;
using Umbraco.Extensions;

namespace N3O.Umbraco.Search.Google;

public class Searcher : ISearcher {
    private const int PageSize = 10;

    private readonly IContentCache _contentCache;
    private readonly IAppPolicyCache _appCache;

    public Searcher(IContentCache contentCache, IAppPolicyCache appCache) {
        _contentCache = contentCache;
        _appCache = appCache;
    }

    public DynamicPager<SearchResult> Search(string query, string currentUrl) {
        var totalResults = (int) GetCachedResults(query, 1, PageSize).TotalResults;

        var pager = new DynamicPager<SearchResult>(new Uri(currentUrl),
                                                   (start, num) => GetCachedResults(query, start, num).Results,
                                                   totalResults,
                                                   PageSize);

        return pager;
    }

    private (long TotalResults, IEnumerable<SearchResult> Results) GetCachedResults(string query,
                                                                                    int start,
                                                                                    int number) {
        var cacheKey = CacheKey.Generate<Searcher>(nameof(Search), query, start, number);

        return _appCache.GetCacheItem(cacheKey, () => ExecuteQuery(query, start, number), TimeSpan.FromHours(1));
    }

    private (long TotalResults, IEnumerable<SearchResult> Results) ExecuteQuery(string query,
                                                                                int start,
                                                                                int number) {
        try {
            var searchSettings = _contentCache.Single<GoogleSearchSettingsContent>();
            var urlSettingsContent = _contentCache.Single<UrlSettingsContent>();
            var siteUrl = urlSettingsContent.ProductionBaseUrl;

            var searcher = new CustomSearchAPIService(new BaseClientService.Initializer {
                ApiKey = searchSettings.ApiKey,
                HttpClientInitializer = new CustomHttpClientInitializer(siteUrl)
            });

            var listRequest = searcher.Cse.List();

            listRequest.Q = query;
            listRequest.Start = start;
            listRequest.Num = number;
            listRequest.Cx = searchSettings.SearchEngineId;

            var search = listRequest.Execute();
            var results = search.Items
                                .OrEmpty()
                                .Select(x => new SearchResult(x.HtmlTitle, x.HtmlSnippet, x.DisplayLink, x.Link))
                                .ToList();

            return (Math.Max((search.SearchInformation.TotalResults.TryParseAs<int>() ?? 0) - 1, 0), results);
        } catch {
            return (0, []);
        }
    }
}
