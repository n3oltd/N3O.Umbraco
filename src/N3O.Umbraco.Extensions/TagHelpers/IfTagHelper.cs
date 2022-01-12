using Microsoft.AspNetCore.Razor.TagHelpers;
using N3O.Umbraco.Templates;

namespace N3O.Umbraco.TagHelpers {
    public class IfTagHelper : TagHelper {
        private readonly IStyleContext _styleContext;

        public IfTagHelper(IStyleContext styleContext) {
            _styleContext = styleContext;
        }
        
        public TemplateStyle HasStyle { get; set; }
        
        public override void Process(TagHelperContext context, TagHelperOutput output) {
            if (!_styleContext.Has(HasStyle)) {
                output.SuppressOutput();
            }
        }
    }
}