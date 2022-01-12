using Microsoft.AspNetCore.Razor.TagHelpers;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Templates;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.TagHelpers {
    public class StyleContextTagHelper : TagHelper {
        private readonly IStyleContext _styleContext;

        public StyleContextTagHelper(IStyleContext styleContext) {
            _styleContext = styleContext;
        }
        
        public IPublishedElement Block { get; set; }
        public TemplateStyle ApplyStyle { get; set; }
        public TemplateStyle ApplyStyle2 { get; set; }
        public TemplateStyle ApplyStyle3 { get; set; }
        public TemplateStyle ApplyStyle4 { get; set; }
        public TemplateStyle ApplyStyle5 { get; set; }
        
        public override void Process(TagHelperContext context, TagHelperOutput output) {
            if (Block != null) {
                SetBlockStyles();
            }

            var stylesToApply = new[] {
                    ApplyStyle, ApplyStyle2, ApplyStyle3, ApplyStyle4, ApplyStyle5
            }.ExceptNull().ToList();
            
            foreach (var style in stylesToApply) {
                _styleContext.Push(style);
            }

            output.TagName = null;
        }

        private void SetBlockStyles() {
            var property = Block?.GetProperty("templateStyles");
            
            if (property != null) {
                var styles = (property.GetValue() as IEnumerable<TemplateStyle>).OrEmpty().ToList();

                _styleContext.PushAll(styles);
            }
        }
    }
}