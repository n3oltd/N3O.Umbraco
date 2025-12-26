using Microsoft.AspNetCore.Razor.TagHelpers;
using N3O.Umbraco.Constants;

namespace N3O.Umbraco.TagHelpers;

[HtmlTargetElement(Attributes = AppendClassAttributeName)]
[HtmlTargetElement(Attributes = AppendIfAttributeName)]
public class AppendClassTagHelper : TagHelper {
    private const string AppendClassAttributeName = $"{Prefixes.TagHelpers}append-class";
    private const string AppendIfAttributeName = $"{Prefixes.TagHelpers}append-if";
    
    [HtmlAttributeName(AppendClassAttributeName)]
    public string AppendClass { get; set; }
    
    [HtmlAttributeName(AppendIfAttributeName)]
    public bool AppendIf { get; set; } = true;

    public override void Process(TagHelperContext context, TagHelperOutput output) {
        if (AppendIf) {
            var cssClass = $"{output.Attributes["class"]?.Value} {AppendClass}";

            output.Attributes.SetAttribute("class", cssClass);
        }
    }
}
