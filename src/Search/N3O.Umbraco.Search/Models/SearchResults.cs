using N3O.Umbraco.Utilities;

namespace N3O.Umbraco.Search.Models {
    public class SearchResults {
        public SearchResults(string query, DynamicPager<SearchResult> pager) {
            Query = query;
            Pager = pager;
        }

        public string Query { get; }
        public DynamicPager<SearchResult> Pager { get; }
    }
}