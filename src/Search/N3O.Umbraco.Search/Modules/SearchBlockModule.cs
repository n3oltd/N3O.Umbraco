using N3O.Umbraco.Blocks;
using N3O.Umbraco.Content;
using N3O.Umbraco.Context;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Search.Content;
using N3O.Umbraco.Search.Models;
using System;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Search.Modules {
    public class SearchBlockModule : IBlockModule {
        private static readonly string SearchBlockAlias = AliasHelper<SearchBlockContent>.ContentTypeAlias();

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
            return block.ContentType.Alias.EqualsInvariant(SearchBlockAlias);
        }

        public Task<object> ExecuteAsync(IPublishedElement block, CancellationToken cancellationToken) {
            var query = _queryStringAccessor.Value.GetValue(SearchConstants.QueryString);
            var currentUrl = _currentUrlAccessor.Value.GetEncodedUrl();
            var pager = _searcher.Value.Search(query, currentUrl);
            var searchResults = new SearchResults(query, pager);

            return Task.FromResult<object>(searchResults);
        }

        public string Key => SearchConstants.BlockModuleKeys.Search;
    }
}