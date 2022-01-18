using Microsoft.AspNetCore.Razor.TagHelpers;
using N3O.Umbraco.Analytics.Extensions;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Pages;
using System;

namespace N3O.Umbraco.Analytics.TagHelpers {
    [HtmlTargetElement("n3o-tag-manager")]
    public class TagManagerTagHelper : TagHelper {
        [HtmlAttributeName("model")]
        public IPageViewModel Model { get; set; }
        
        [HtmlAttributeName("render")]
        public string Render { get; set; }
        
        public override void Process(TagHelperContext context, TagHelperOutput output) {
            if (Model == null) {
                throw new ArgumentException(nameof(Model));
            }
            
            if (Render == null) {
                throw new ArgumentException(nameof(Render));
            }

            if (Model.TagManager() == null) {
                output.SuppressOutput();
            } else {
                output.TagName = null;
            
                if (Render.EqualsInvariant("head")) {
                    output.Content.SetHtmlContent(Model.TagManager().Head);
                } else if (Render.EqualsInvariant("body")) {
                    output.Content.SetHtmlContent(Model.TagManager().Body);
                } else {
                    throw new Exception($"{nameof(Render)} parameter passed to {nameof(TagManagerTagHelper)} must be head or body");
                }
            }
        }
    }
}