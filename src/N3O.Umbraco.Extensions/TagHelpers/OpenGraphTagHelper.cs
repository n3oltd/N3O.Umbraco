using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;
using N3O.Umbraco.Constants;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Pages;
using System;
using Umbraco.Extensions;

namespace N3O.Umbraco.TagHelpers;

[HtmlTargetElement($"{Prefixes.TagHelpers}open-graph")]
public class OpenGraphTagHelper : TagHelper {
    [HtmlAttributeName("model")]
    public IPageViewModel Model { get; set; }

    public override void Process(TagHelperContext context, TagHelperOutput output) {
        if (Model == null) {
            throw new ArgumentException(nameof(Model));
        }

        var openGraphData = Model.OpenGraph();

        output.TagName = null;

        AddOpenGraphTag(output, "title", openGraphData?.Title);
        AddOpenGraphTag(output, "description", openGraphData?.Description);
        AddOpenGraphTag(output, "url", openGraphData?.Url ?? Model.Content.AbsoluteUrl());
        AddOpenGraphTag(output, "image", openGraphData?.ImageUrl);
    }

    private void AddOpenGraphTag(TagHelperOutput output, string property, string content) {
        if (content.HasValue()) {
            var metaTag = new TagBuilder("meta");
            metaTag.Attributes.Add("property", $"og:{property}");
            metaTag.Attributes.Add("content", content);
        
            output.Content.AppendHtml(metaTag.ToHtmlString());
        }
    }
}
