using Microsoft.AspNetCore.Razor.TagHelpers;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Templates;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.TagHelpers;

[HtmlTargetElement("n3o-style-context")]
public class StyleContextTagHelper : TagHelper {
    private readonly IStyleContext _styleContext;

    public StyleContextTagHelper(IStyleContext styleContext) {
        _styleContext = styleContext;
    }
    
    [HtmlAttributeName("for-block")]
    public IPublishedElement ForBlock { get; set; }
    
    [HtmlAttributeName("with")]
    public TemplateStyle With { get; set; }
    
    [HtmlAttributeName("with2")]
    public TemplateStyle With2 { get; set; }
    
    [HtmlAttributeName("with3")]
    public TemplateStyle With3 { get; set; }
    
    [HtmlAttributeName("with4")]
    public TemplateStyle With4 { get; set; }
    
    [HtmlAttributeName("with5")]
    public TemplateStyle With5 { get; set; }
    
    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output) {
        if (ForBlock != null) {
            SetBlockStyles();
        }

        var stylesToApply = new[] { With, With2, With3, With4, With5 }.ExceptNull().ToList();
        
        foreach (var style in stylesToApply) {
            _styleContext.Push(style);
        }

        var tagHelperContent = output.IsContentModified ? output.Content : await output.GetChildContentAsync();
        output.TagName = null;
        output.Content.SetHtmlContent(tagHelperContent.GetContent());
        
        _styleContext.Pop(stylesToApply.Count);
    }

    private void SetBlockStyles() {
        var property = ForBlock?.GetProperty("templateStyles");
        
        if (property != null) {
            var styles = (property.GetValue() as IEnumerable<TemplateStyle>).OrEmpty().ToList();

            _styleContext.PushAll(styles);
        }
    }
}
