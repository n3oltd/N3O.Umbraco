using Microsoft.AspNetCore.Razor.TagHelpers;
using N3O.Umbraco.Analytics.Extensions;
using N3O.Umbraco.Constants;
using N3O.Umbraco.Pages;
using System;

namespace N3O.Umbraco.Analytics.TagHelpers;

[HtmlTargetElement($"{Prefixes.TagHelpers}google-analytics-4")]
public class GoogleAnalytics4TagHelper : TagHelper {
    [HtmlAttributeName("model")]
    public IPageViewModel Model { get; set; }

    public override void Process(TagHelperContext context, TagHelperOutput output) {
        if (Model == null) {
            throw new ArgumentException(nameof(Model));
        }

        if (Model.GoogleAnalytics4() == null) {
            output.SuppressOutput();
        } else {
            output.TagName = null;
        
            output.Content.SetHtmlContent(Model.GoogleAnalytics4().JavaScript);
        }
    }
}
