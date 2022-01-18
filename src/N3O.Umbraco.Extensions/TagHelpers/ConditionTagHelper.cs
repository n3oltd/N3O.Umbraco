using Microsoft.AspNetCore.Razor.TagHelpers;

namespace N3O.Umbraco.TagHelpers {
    [HtmlTargetElement(Attributes = ConditionAttributeName)]
    public class ConditionTagHelper : TagHelper {
        public const string ConditionAttributeName = "n3o-condition";
        
        [HtmlAttributeName(ConditionAttributeName)]
        public bool Condition { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output) {
            if (!Condition) {
                output.SuppressOutput();
            }
        }
    }
}