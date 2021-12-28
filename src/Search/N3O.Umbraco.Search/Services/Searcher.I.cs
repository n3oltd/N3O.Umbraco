using N3O.Umbraco.Search.Models;
using N3O.Umbraco.Utilities;
using System;

namespace N3O.Umbraco.Search;

public interface ISearcher {
    DynamicPager<SearchResult> Search(string query, Uri currentUrl);
}
