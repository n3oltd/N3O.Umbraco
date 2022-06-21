using Microsoft.AspNetCore.Razor.TagHelpers;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Templates;
using System.Text;

namespace N3O.Umbraco.TagHelpers;

[HtmlTargetElement(Attributes = ApplyStylesAttributeName)]
public class ApplyStylesTagHelper : TagHelper {
    private const string ApplyStylesAttributeName = "n3o-styles";
    
    private readonly IStyleContext _styleContext;

    public ApplyStylesTagHelper(IStyleContext styleContext) {
        _styleContext = styleContext;
    }
    
    [HtmlAttributeName(ApplyStylesAttributeName)]
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

            output.Attributes.SetAttribute("class", sb.ToString().Trim());
        }
    }
}
