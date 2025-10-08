using HtmlAgilityPack;

namespace N3O.Umbraco.Blocks.Extensions;

public static class StringExtensions {
    public static string CleanUpMarkupForPreview(this string markup) {
        if (string.IsNullOrWhiteSpace(markup)) {
            return markup;
        }

        var content = new HtmlDocument();
        content.LoadHtml(markup);

        var links = content.DocumentNode.SelectNodes("//a");

        if (links != null) {
            foreach (var link in links) {
                link.SetAttributeValue("href", "javascript:;");
            }
        }

        var formElements = content.DocumentNode.SelectNodes("//input | //textarea | //select | //button");
        
        if (formElements != null) {
            foreach (var formElement in formElements) {
                formElement.SetAttributeValue("disabled", "disabled");
            }
        }

        return content.DocumentNode.OuterHtml;
    }
}