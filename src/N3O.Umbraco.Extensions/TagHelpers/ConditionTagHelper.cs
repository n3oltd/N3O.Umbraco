using Microsoft.AspNetCore.Razor.TagHelpers;
using N3O.Umbraco.Constants;

namespace N3O.Umbraco.TagHelpers;

[HtmlTargetElement(Attributes = ConditionAttributeName)]
public class ConditionTagHelper : TagHelper {
    private const string ConditionAttributeName = $"{Prefixes.TagHelpers}condition";
    
    [HtmlAttributeName(ConditionAttributeName)]
    public bool Condition { get; set; }

    public override void Process(TagHelperContext context, TagHelperOutput output) {
        if (!Condition) {
            output.SuppressOutput();
        }
    }
}
