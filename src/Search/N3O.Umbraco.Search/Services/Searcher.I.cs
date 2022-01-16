using N3O.Umbraco.Search.Models;
using N3O.Umbraco.Utilities;

namespace N3O.Umbraco.Search {
    public interface ISearcher {
        DynamicPager<SearchResult> Search(string query, string currentUrl);
    }
}
