using AngleSharp;
using AngleSharp.Dom;
using N3O.Umbraco.Extensions;
using System.Text;

namespace N3O.Umbraco.Search;

public static class HtmlToText {
    public static string GetText(string html, string querySelector = null) {
        var context = BrowsingContext.New();
        var document = context.OpenAsync(req => req.Content(html)).GetAwaiter().GetResult();

        var text = new StringBuilder();
        
        if (querySelector == null) {
            text.Append(document.Body.Text());
        } else {
            var elements = document.QuerySelectorAll(querySelector);

            foreach (var element in elements) {
                text.Append(element.Text());
            }
        }
        
        return text.ToString().TrimOrNull();
    }
}