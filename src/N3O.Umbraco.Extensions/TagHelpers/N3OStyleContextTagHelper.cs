using Microsoft.AspNetCore.Razor.TagHelpers;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Templates;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.TagHelpers {
    public class N3OStyleContextTagHelper : TagHelper {
        private readonly IStyleContext _styleContext;

        public N3OStyleContextTagHelper(IStyleContext styleContext) {
            _styleContext = styleContext;
        }
        
        
        public IPublishedElement ForBlock { get; set; }
        public TemplateStyle With { get; set; }
        public TemplateStyle With2 { get; set; }
        public TemplateStyle With3 { get; set; }
        public TemplateStyle With4 { get; set; }
        public TemplateStyle With5 { get; set; }
        
        public override void Process(TagHelperContext context, TagHelperOutput output) {
            if (ForBlock != null) {
                SetBlockStyles();
            }

            var stylesToApply = new[] { With, With2, With3, With4, With5 }.ExceptNull().ToList();
            
            foreach (var style in stylesToApply) {
                _styleContext.Push(style);
            }

            output.TagName = null;
        }

        private void SetBlockStyles() {
            var property = ForBlock?.GetProperty("templateStyles");
            
            if (property != null) {
                var styles = (property.GetValue() as IEnumerable<TemplateStyle>).OrEmpty().ToList();

                _styleContext.PushAll(styles);
            }
        }
    }
}