using Humanizer;
using Microsoft.AspNetCore.Razor.TagHelpers;
using N3O.Umbraco.Constants;
using N3O.Umbraco.Crowdfunding.Models;
using N3O.Umbraco.Data.Lookups;

namespace N3O.Umbraco.Crowdfunding.TagHelpers;

[HtmlTargetElement(Attributes = EditorAttributeName)]
public class EditorTagHelper : TagHelper {
    private const string EditorAttributeName = $"{Prefixes.TagHelpers}editor";
    
    [HtmlAttributeName(EditorAttributeName)]
    public (ICrowdfunderViewModel Model, PropertyType Type, string Alias) Parameters { get; set; }
    
    public override void Process(TagHelperContext context, TagHelperOutput output) {
        if (Parameters.Model.EditMode()) {
            output.Attributes.SetAttribute("data-type", Parameters.Type.DataTypeAttributeValue);
            output.Attributes.SetAttribute("data-property-alias", Parameters.Alias.Camelize());
        }
    }
}
