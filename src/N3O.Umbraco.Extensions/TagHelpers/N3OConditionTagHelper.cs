using Microsoft.AspNetCore.Razor.TagHelpers;

namespace N3O.Umbraco.TagHelpers {
    [HtmlTargetElement(Attributes = nameof(Condition))]
    public class N3OConditionTagHelper : TagHelper {
        [HtmlAttributeName("n3o-condition")]
        public bool Condition { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output) {
            if (!Condition) {
                output.SuppressOutput();
            }
        }
    }
}