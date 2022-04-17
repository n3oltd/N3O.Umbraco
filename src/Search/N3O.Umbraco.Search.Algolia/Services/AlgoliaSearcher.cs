using N3O.Umbraco.Search.Models;
using N3O.Umbraco.Utilities;
using System;

namespace N3O.Umbraco.Search.Algolia {
    public class AlgoliaSearcher : ISearcher {
        public DynamicPager<SearchResult> Search(string query, string currentUrl) {
            throw new NotImplementedException();
        }
    }
}