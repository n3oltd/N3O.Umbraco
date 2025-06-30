using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;
using N3O.Umbraco.Content;
using N3O.Umbraco.Crowdfunding.Content;
using N3O.Umbraco.Extensions;
using Umbraco.Extensions;

namespace N3O.Umbraco.Crowdfunding.TagHelpers;

[HtmlTargetElement("n3o-crowdfunding-css")]
public class CrowdfundingCssTagHelper : TagHelper {
    private readonly IContentCache _contentCache;

    public CrowdfundingCssTagHelper(IContentCache contentCache) {
        _contentCache = contentCache;
    }

    public override void Process(TagHelperContext context, TagHelperOutput output) {
        var templateSettings = _contentCache.Single<TemplateSettingsContent>();

        if (templateSettings != null) {
            output.TagName = null;
            
            AddStyleTag(output, templateSettings.CssVariables);
        } else {
            output.SuppressOutput();
        }
    }

    private void AddStyleTag(TagHelperOutput output, string cssVariables) {
        if (cssVariables.HasValue()) {
            var styleTag = new TagBuilder("style");
            styleTag.Attributes.Add("type", "text/css");
            styleTag.InnerHtml.AppendHtmlLine(cssVariables);
        
            output.Content.AppendHtml(styleTag.ToHtmlString());
        }
    }
}
