using Microsoft.AspNetCore.Razor.TagHelpers;
using N3O.Umbraco.Extensions;

namespace N3O.Umbraco.TagHelpers;

[HtmlTargetElement("n3o-link-wrapper")]
public class LinkWrapperTagHelper : TagHelper {
    [HtmlAttributeName("href")]
    public string Href { get; set; }
    
    [HtmlAttributeName("title")]
    public string Title { get; set; }
    
    public override void Process(TagHelperContext context, TagHelperOutput output) {
        if (Href.HasValue()) {
            output.TagName = "a";
            output.Attributes.Add("href", Href);

            if (Title.HasValue()) {
                output.Attributes.Add("title", Title);
            }
        } else {
            output.TagName = null;
        }
    }
}