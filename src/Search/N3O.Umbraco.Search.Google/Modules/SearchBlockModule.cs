using N3O.Umbraco.Blocks;
using N3O.Umbraco.Content;
using N3O.Umbraco.Context;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Search.Content;
using N3O.Umbraco.Search.Google.Models;
using System;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Search.Google.Modules;

public class SearchBlockModule : IBlockModule {
    private static readonly string BlockAlias = AliasHelper<SearchBlockContent>.ContentTypeAlias();

    private readonly Lazy<ISearcher> _searcher;
    private readonly Lazy<IQueryStringAccessor> _queryStringAccessor;
    private readonly Lazy<ICurrentUrlAccessor> _currentUrlAccessor;

    public SearchBlockModule(Lazy<ISearcher> searcher,
                             Lazy<IQueryStringAccessor> queryStringAccessor,
                             Lazy<ICurrentUrlAccessor> currentUrlAccessor) {
        _searcher = searcher;
        _queryStringAccessor = queryStringAccessor;
        _currentUrlAccessor = currentUrlAccessor;
    }
    
    public bool ShouldExecute(IPublishedElement block) {
        return block.ContentType.Alias.EqualsInvariant(BlockAlias);
    }

    public Task<object> ExecuteAsync(IPublishedElement block, CancellationToken cancellationToken) {
        var query = _queryStringAccessor.Value.GetValue(SearchConstants.QueryString);

        if (query.HasValue()) {
            var currentUrl = _currentUrlAccessor.Value.GetEncodedUrl();
            var pager = _searcher.Value.Search(query, currentUrl);
            var searchResults = new SearchResults(query, pager);

            return Task.FromResult<object>(searchResults);
        } else {
            return Task.FromResult<object>(null);
        }
    }

    public string Key => SearchConstants.BlockModuleKeys.Search;
}
