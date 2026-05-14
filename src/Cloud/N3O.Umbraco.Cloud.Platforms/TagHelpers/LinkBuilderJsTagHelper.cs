using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;
using N3O.Umbraco.Constants;
using Umbraco.Extensions;

namespace N3O.Umbraco.Cloud.Platforms.TagHelpers;

[HtmlTargetElement($"{Prefixes.TagHelpers}link-builder-js")]
public class LinkBuilderJsTagHelper : TagHelper {
    private readonly ICloudUrl _cloudUrl;

    public LinkBuilderJsTagHelper(ICloudUrl cloudUrl) {
        _cloudUrl = cloudUrl;
    }

    public override void Process(TagHelperContext context, TagHelperOutput output) {
        output.TagName = null;

        var scriptTag = new TagBuilder("script");
        scriptTag.Attributes.Add("src", _cloudUrl.ForLinkBuilder("js/embed.js"));

        output.Content.AppendHtmlLine("<!-- N3O Link Builder JS (https://n3o.ltd) -->");
        output.Content.AppendHtml(scriptTag.ToHtmlString());
    }
}
