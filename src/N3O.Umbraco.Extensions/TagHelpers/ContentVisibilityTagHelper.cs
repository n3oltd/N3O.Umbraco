using Microsoft.AspNetCore.Razor.TagHelpers;
using N3O.Umbraco.Constants;
using N3O.Umbraco.Content;
using N3O.Umbraco.Pages;

namespace N3O.Umbraco.TagHelpers;

[HtmlTargetElement($"{Prefixes.TagHelpers}content-visibility")]
public class ContentVisibilityTagHelper : TagHelper {
    private readonly IContentVisibility _contentVisibility;

    public ContentVisibilityTagHelper(IContentVisibility contentVisibility) {
        _contentVisibility = contentVisibility;
    }

    [HtmlAttributeName("model")]
    public IPageViewModel Model { get; set; }
    
    public override void Process(TagHelperContext context, TagHelperOutput output) {
        var isVisible = _contentVisibility.IsVisible(Model.Content);

        if (isVisible) {
            output.SuppressOutput();
        } else {
            output.TagName = "meta";
            output.TagMode = TagMode.SelfClosing;
            
            output.Attributes.SetAttribute("name", "robots");
            output.Attributes.SetAttribute("content", "noindex");
        }
    }
}
