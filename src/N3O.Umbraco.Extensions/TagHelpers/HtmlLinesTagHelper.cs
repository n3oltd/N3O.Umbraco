using Microsoft.AspNetCore.Razor.TagHelpers;
using N3O.Umbraco.Extensions;
using System.Collections.Generic;

namespace N3O.Umbraco.TagHelpers;

[HtmlTargetElement("n3o-html-lines")]
public class HtmlLinesTagHelper : TagHelper {
    [HtmlAttributeName("single")]
    public string Single { get; set; }
    
    [HtmlAttributeName("multiple")]
    public IEnumerable<string> Multiple { get; set; }
    
    public override void Process(TagHelperContext context, TagHelperOutput output) {
        output.TagName = null;

        var html = Single.HasValue()
                       ? Single.Replace("\r", "").Replace("\n", "<br>")
                       : Multiple.OrEmpty().Join("<br>");
        
        output.Content.SetHtmlContent(html);
    }
}
