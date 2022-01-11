using Microsoft.AspNetCore.Razor.TagHelpers;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Lookups;
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
        
        public override void Process(TagHelperContext context, TagHelperOutput output) {
            var property = Block?.GetProperty("templateStyles");
            
            if (property != null) {
                var styles = (property.GetValue() as IEnumerable<TemplateStyle>).OrEmpty().ToList();

                _styleContext.PushAll(styles);
            }
            
            output.TagName = null;
        }
    }
}