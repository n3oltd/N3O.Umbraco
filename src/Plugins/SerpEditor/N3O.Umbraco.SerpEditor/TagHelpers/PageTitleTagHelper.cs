using Microsoft.AspNetCore.Razor.TagHelpers;
using N3O.Umbraco.Pages;
using N3O.Umbraco.SerpEditor.Extensions;
using System.Web;

namespace N3O.Umbraco.SerpEditor.TagHelpers;

[HtmlTargetElement("n3o-page-title")]
public class PageTitleTagHelper : TagHelper {
    [HtmlAttributeName("model")]
    public IPageViewModel Model { get; set; }
    
    public override void Process(TagHelperContext context, TagHelperOutput output) {
        output.TagName = "title";
        output.TagMode = TagMode.StartTagAndEndTag;

        output.Content.SetHtmlContent(HttpUtility.HtmlEncode(Model.SerpEntry().Title));
    }
}
