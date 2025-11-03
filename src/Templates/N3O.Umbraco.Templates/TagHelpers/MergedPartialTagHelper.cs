using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Threading.Tasks;

namespace N3O.Umbraco.Templates.TagHelpers;

[HtmlTargetElement("n3o-merged-partial")]
public class MergedPartialTagHelper : TagHelper {
    private readonly IMerger _merger;

    public MergedPartialTagHelper(IMerger merger) {
        _merger = merger;
    }
    
    [HtmlAttributeName("view")]
    public string View { get; set; }
    
    [HtmlAttributeName("view-context")]
    public ViewContext ViewContext { get; set; }
    
    [HtmlAttributeName("model")]
    public object Model { get; set; }

    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output) {
        var htmlContent = await _merger.MergePartialForCurrentContentAsync(ViewContext, View, Model);

        output.TagName = null;
        output.Content.AppendHtml(htmlContent);
    }
}
