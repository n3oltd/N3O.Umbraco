using Microsoft.AspNetCore.Razor.TagHelpers;
using N3O.Umbraco.Analytics.Extensions;
using N3O.Umbraco.Constants;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Pages;
using System;

namespace N3O.Umbraco.Analytics.TagHelpers;

[HtmlTargetElement($"{Prefixes.TagHelpers}google-tag-manager")]
public class GoogleTagManagerTagHelper : TagHelper {
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

        if (Model.GoogleTagManager() == null) {
            output.SuppressOutput();
        } else {
            output.TagName = null;
        
            if (Render.EqualsInvariant("head")) {
                output.Content.SetHtmlContent(Model.GoogleTagManager().Head);
            } else if (Render.EqualsInvariant("body")) {
                output.Content.SetHtmlContent(Model.GoogleTagManager().Body);
            } else {
                throw new Exception($"{nameof(Render)} parameter passed to {nameof(GoogleTagManagerTagHelper)} must be head or body");
            }
        }
    }
}
