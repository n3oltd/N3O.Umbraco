using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;
using N3O.Umbraco.Constants;
using Umbraco.Extensions;

namespace MuslimHands.Core.TagHelpers;

[HtmlTargetElement($"{Prefixes.TagHelpers}platforms-checkout-mode")]
public class PlatformsSameSiteCheckoutModeTagHelper : TagHelper {
    public override void Process(TagHelperContext context, TagHelperOutput output) {
        output.TagName = null;
        
        var metaTag = new TagBuilder("meta");
        metaTag.MergeAttribute("name", "n3o-checkout-same-site");
        metaTag.MergeAttribute("content", "true");

        metaTag.TagRenderMode = TagRenderMode.SelfClosing;

        output.Content.AppendHtml(metaTag.ToHtmlString());
    }
}