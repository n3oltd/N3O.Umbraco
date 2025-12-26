using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;
using N3O.Umbraco.Constants;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Pages;
using System.Linq;

namespace N3O.Umbraco.TagHelpers;

[HtmlTargetElement($"{Prefixes.TagHelpers}metadata-entries")]
public class MetadataEntriesTagHelper : TagHelper {
    [HtmlAttributeName("model")]
    public IPageViewModel Model { get; set; }
    
    public override void Process(TagHelperContext context, TagHelperOutput output) {
        var entries = Model.MetadataEntries();
        
        if (entries.Any()) {
            output.TagName = null;

            foreach (var entry in entries) {
                var metaTag = new TagBuilder("meta");
                
                metaTag.Attributes.Add("name", entry.Name);
                metaTag.Attributes.Add("content", entry.Content);

                output.Content.AppendHtml(metaTag);
            }
        } else {
            output.SuppressOutput();
        }
    }
}
