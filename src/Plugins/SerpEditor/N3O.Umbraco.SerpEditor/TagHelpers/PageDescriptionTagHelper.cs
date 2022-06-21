using Microsoft.AspNetCore.Razor.TagHelpers;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Pages;
using N3O.Umbraco.SerpEditor.Extensions;

namespace N3O.Umbraco.SerpEditor.TagHelpers;

[HtmlTargetElement("n3o-page-description")]
public class PageDescriptionTagHelper : TagHelper {
    [HtmlAttributeName("model")]
    public IPageViewModel Model { get; set; }
    
    public override void Process(TagHelperContext context, TagHelperOutput output) {
        var description = Model.SerpEntry().Description;

        if (description.HasValue()) {
            output.TagName = "meta";
            output.TagMode = TagMode.SelfClosing;
            
            output.Attributes.SetAttribute("name", "description");
            output.Attributes.SetAttribute("content", description);
        } else {
            output.SuppressOutput();
        }
    }
}
