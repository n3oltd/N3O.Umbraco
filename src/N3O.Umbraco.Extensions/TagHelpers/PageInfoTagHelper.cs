using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Razor.TagHelpers;
using N3O.Umbraco.Pages;
using NodaTime;
using NodaTime.Text;
using System;

namespace N3O.Umbraco.TagHelpers;

[HtmlTargetElement("n3o-page-info")]
public class PageInfoTagHelper : TagHelper {
    private readonly IClock _clock;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public PageInfoTagHelper(IClock clock, IWebHostEnvironment webHostEnvironment) {
        _clock = clock;
        _webHostEnvironment = webHostEnvironment;
    }
    
    [HtmlAttributeName("model")]
    public IPageViewModel Model { get; set; }

    public override void Process(TagHelperContext context, TagHelperOutput output) {
        if (Model == null) {
            throw new ArgumentException(nameof(Model));
        }
        
        var now = InstantPattern.General.Format(_clock.GetCurrentInstant());
        var currentEnvironment = _webHostEnvironment.EnvironmentName;
        
        output.TagName = null;

        output.Content.SetHtmlContent($"<!-- {Model.Content?.Id} {now} {currentEnvironment} -->");
    }
}
