using AngleSharp;
using AngleSharp.Dom;
using N3O.Umbraco.Extensions;
using System.Text;
using System.Text.RegularExpressions;

namespace N3O.Umbraco.Search;

public static class HtmlToText {
    public static string GetText(string html, string querySelector = null) {
        var context = BrowsingContext.New();
        var document = context.OpenAsync(req => req.Content(html)).GetAwaiter().GetResult();

        
        foreach (var node in document.QuerySelectorAll("script, style, noscript")) {
            node.Remove();
        }
        
        var sb = new StringBuilder();
        
        if (querySelector == null) {
            sb.Append(document.Body.Text());
        } else {
            var elements = document.QuerySelectorAll(querySelector);

            foreach (var element in elements) {
                sb.Append(element.Text());
            }
        }
        
        var text = sb.ToString().TrimOrNull() ?? string.Empty;
        
        return Regex.Replace(text, @"\s+", " ");
    }
}