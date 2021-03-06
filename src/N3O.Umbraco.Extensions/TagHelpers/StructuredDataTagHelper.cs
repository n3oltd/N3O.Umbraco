using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Razor.TagHelpers;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Pages;
using System;

namespace N3O.Umbraco.TagHelpers;

[HtmlTargetElement("n3o-structured-data")]
public class StructuredDataTagHelper : TagHelper {
    [HtmlAttributeName("model")]
    public IPageViewModel Model { get; set; }

    public override void Process(TagHelperContext context, TagHelperOutput output) {
        if (Model == null) {
            throw new ArgumentException(nameof(Model));
        }

        if (Model.StructuredData() == null) {
            output.SuppressOutput();
        } else {
            output.TagName = null;

            // Don't use tag builder as that HTML encodes the type attribute value
            output.Content.AppendHtmlLine(@"<script type=""application/ld+json"">");
            output.Content.AppendHtml(Model.StructuredData().JavaScriptObject);
            output.Content.AppendHtmlLine("</script>");
        }
    }
}
