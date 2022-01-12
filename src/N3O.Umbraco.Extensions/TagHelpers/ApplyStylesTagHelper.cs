using Microsoft.AspNetCore.Razor.TagHelpers;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Templates;
using System.Text;

namespace N3O.Umbraco.TagHelpers {
    [HtmlTargetElement(Attributes = nameof(ApplyStyles))]
    public class ApplyStylesTagHelper : TagHelper {
        private readonly IStyleContext _styleContext;

        public ApplyStylesTagHelper(IStyleContext styleContext) {
            _styleContext = styleContext;
        }
        
        public bool ApplyStyles { get; set; }
        
        public override void Process(TagHelperContext context, TagHelperOutput output) {
            if (ApplyStyles) {
                var sb = new StringBuilder();

                sb.Append(output.Attributes["class"]?.Value ?? "");

                foreach (var style in _styleContext.GetAll()) {
                    if (style.CssClass.HasValue()) {
                        sb.Append($" {style.CssClass}");
                    }
                }

                output.Attributes.RemoveWhere(x => x.Name == "class");
                output.Attributes.Add("class", sb.ToString().Trim());
            }
        }
    }
}