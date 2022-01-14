using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Pages;
using System;
using Umbraco.Extensions;

namespace N3O.Umbraco.TagHelpers {
    [HtmlTargetElement("n3o-structured-data")]
    public class StructuredDataTagHelper : TagHelper {
        public IPageViewModel Model { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output) {
            if (Model == null) {
                throw new ArgumentException(nameof(Model));
            }

            if (Model.StructuredData() == null) {
                output.SuppressOutput();
            } else {
                output.TagName = null;
            
                var scriptTag = new TagBuilder("script");
            
                scriptTag.Attributes.Add("type", "application/ld+json");
            
                scriptTag.InnerHtml.AppendHtml(Model.StructuredData().JavaScriptObject);
            
                output.Content.SetHtmlContent(scriptTag.ToHtmlString());   
            }
        }
    }
}