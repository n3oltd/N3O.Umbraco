using Microsoft.AspNetCore.Razor.TagHelpers;
using N3O.Umbraco.Extensions;
using System.Collections.Generic;

namespace N3O.Umbraco.TagHelpers {
    [HtmlTargetElement("n3o-html-lines")]
    public class HtmlLinesTagHelper : TagHelper {
        public IEnumerable<string> Text { get; set; }
        
        public override void Process(TagHelperContext context, TagHelperOutput output) {
            output.TagName = null;

            var html = Text.OrEmpty().Join("<br>");
            
            output.Content.SetHtmlContent(html);
        }
    }
}