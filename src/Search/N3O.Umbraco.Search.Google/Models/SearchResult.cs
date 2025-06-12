using Microsoft.AspNetCore.Html;
using N3O.Umbraco.Extensions;

namespace N3O.Umbraco.Search.Google.Models;

public class SearchResult : Value {
    public SearchResult(string title, string snippet, string displayUrl, string url) {
        Title = title?.ToHtmlString();
        Snippet = snippet?.ToHtmlString();
        DisplayUrl = displayUrl?.ToHtmlString();
        Url = url;
    }

    public HtmlString Title { get; }
    public HtmlString Snippet { get; }
    public HtmlString DisplayUrl { get; }
    public string Url { get; }
}
