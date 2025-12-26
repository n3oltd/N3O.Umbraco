using Microsoft.AspNetCore.Razor.TagHelpers;
using N3O.Umbraco.Constants;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Pages;

namespace N3O.Umbraco.TagHelpers;

[HtmlTargetElement($"{Prefixes.TagHelpers}canonical-url")]
public class CanonicalUrlTagHelper : TagHelper {
    [HtmlAttributeName("model")]
    public IPageViewModel Model { get; set; }

    public override void Process(TagHelperContext context, TagHelperOutput output) {
        output.TagName = "link";
        output.TagMode = TagMode.SelfClosing;
        
        output.Attributes.SetAttribute("href", Model.Content.AbsoluteUrl());
    }
}
