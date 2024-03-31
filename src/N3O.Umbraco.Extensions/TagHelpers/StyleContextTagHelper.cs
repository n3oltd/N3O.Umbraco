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
    public ITemplateStyle With { get; set; }
    
    [HtmlAttributeName("with2")]
    public ITemplateStyle With2 { get; set; }
    
    [HtmlAttributeName("with3")]
    public ITemplateStyle With3 { get; set; }
    
    [HtmlAttributeName("with4")]
    public ITemplateStyle With4 { get; set; }
    
    [HtmlAttributeName("with5")]
    public ITemplateStyle With5 { get; set; }
    
    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output) {
        var blockStyles = new List<ITemplateStyle>();
        
        if (ForBlock != null) {
            blockStyles.AddRange(GetBlockStyles());
        }

        var stylesToApply = blockStyles.Concat(With)
                                       .Concat(With2)
                                       .Concat(With3)
                                       .Concat(With4)
                                       .Concat(With5)
                                       .ExceptNull()
                                       .ToList();
        
        foreach (var style in stylesToApply) {
            _styleContext.Push(style);
        }

        var tagHelperContent = output.IsContentModified ? output.Content : await output.GetChildContentAsync();
        output.TagName = null;
        output.Content.SetHtmlContent(tagHelperContent.GetContent());
        
        _styleContext.Pop(stylesToApply.Count);
    }

    private IEnumerable<ITemplateStyle> GetBlockStyles() {
        var property = ForBlock?.GetProperty("templateStyles");
        
        if (property != null) {
            var styles = (property.GetValue() as IEnumerable<ITemplateStyle>).OrEmpty().ToList();

            return styles;
        } else {
            return Enumerable.Empty<ITemplateStyle>();
        }
    }
}
