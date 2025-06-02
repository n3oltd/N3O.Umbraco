using N3O.Umbraco.Search.Google.Models;
using N3O.Umbraco.Utilities;

namespace N3O.Umbraco.Search.Google;

public interface ISearcher {
    DynamicPager<SearchResult> Search(string query, string currentUrl);
}
