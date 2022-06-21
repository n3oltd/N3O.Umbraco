using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;
using N3O.Umbraco.Analytics.Extensions;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Pages;
using System;
using Umbraco.Extensions;

namespace N3O.Umbraco.Analytics.TagHelpers;

[HtmlTargetElement("n3o-data-layer")]
public class DataLayerTagHelper : TagHelper {
    [HtmlAttributeName("model")]
    public IPageViewModel Model { get; set; }

    public override void Process(TagHelperContext context, TagHelperOutput output) {
        if (Model == null) {
            throw new ArgumentException(nameof(Model));
        }

        if (!Model.DataLayer().HasValue(x => x.JavaScript)) {
            output.SuppressOutput();
        } else {
            output.TagName = null;

            var scriptTag = new TagBuilder("script");

            scriptTag.InnerHtml.AppendHtmlLine("window.dataLayer = window.dataLayer || [];");
            scriptTag.InnerHtml.AppendHtml(Model.DataLayer().JavaScript);

            output.Content.SetHtmlContent(scriptTag.ToHtmlString());
        }
    }
}
